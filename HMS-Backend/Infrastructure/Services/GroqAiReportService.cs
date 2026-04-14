using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HMS_Backend.Application.DTOs.AiReport;
using HMS_Backend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HMS_Backend.Infrastructure.Services;

public class GroqAiReportService : IAiReportService
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<GroqAiReportService> _logger;
    private readonly string _apiKey;
    private const string GroqApiUrl = "https://api.groq.com/openai/v1/chat/completions";
    private const string Model = "llama-3.3-70b-versatile";

    private static readonly IReadOnlyList<SupportedLanguageDto> Languages = new List<SupportedLanguageDto>
    {
        new() { Code = "en", Name = "English",    NativeName = "English" },
        new() { Code = "bn", Name = "Bengali",    NativeName = "বাংলা" },
        new() { Code = "hi", Name = "Hindi",      NativeName = "हिन्दी" },
        new() { Code = "es", Name = "Spanish",    NativeName = "Español" },
        new() { Code = "fr", Name = "French",     NativeName = "Français" },
        new() { Code = "ar", Name = "Arabic",     NativeName = "العربية" },
        new() { Code = "zh", Name = "Chinese",    NativeName = "中文" },
        new() { Code = "pt", Name = "Portuguese", NativeName = "Português" },
        new() { Code = "de", Name = "German",     NativeName = "Deutsch" },
        new() { Code = "ja", Name = "Japanese",   NativeName = "日本語" },
        new() { Code = "ko", Name = "Korean",     NativeName = "한국어" },
        new() { Code = "ru", Name = "Russian",    NativeName = "Русский" },
        new() { Code = "tr", Name = "Turkish",    NativeName = "Türkçe" },
        new() { Code = "ur", Name = "Urdu",       NativeName = "اردو" },
        new() { Code = "ta", Name = "Tamil",      NativeName = "தமிழ்" },
    };

    public GroqAiReportService(HttpClient httpClient, IUnitOfWork uow, IConfiguration config, ILogger<GroqAiReportService> logger)
    {
        _httpClient = httpClient;
        _uow = uow;
        _logger = logger;
        _apiKey = config["Groq:ApiKey"];
        if (string.IsNullOrWhiteSpace(_apiKey))
            throw new InvalidOperationException("Groq:ApiKey is not configured. Set it via environment variable 'Groq__ApiKey' or in appsettings.json.");
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public IReadOnlyList<SupportedLanguageDto> GetSupportedLanguages() => Languages;

    public async Task<ReportSummaryResponseDto> SummarizeRecordAsync(Guid medicalRecordId, string language, CancellationToken ct = default)
    {
        var record = await _uow.MedicalRecords.FirstOrDefaultAsync(
            r => r.Id == medicalRecordId && !r.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Medical record '{medicalRecordId}' not found.");

        var langName = ResolveLanguageName(language);

        var prompt = BuildSingleRecordPrompt(record, langName);

        return await CallGroqAsync(prompt, langName, ct);
    }

    public async Task<ReportSummaryResponseDto> SummarizePatientHistoryAsync(Guid patientId, string language, CancellationToken ct = default)
    {
        var patient = await _uow.Patients.FirstOrDefaultAsync(
            p => p.Id == patientId && !p.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Patient '{patientId}' not found.");

        var records = await _uow.MedicalRecords.FindAsync(
            r => r.PatientId == patientId && !r.IsDeleted, ct);

        var recordList = records.OrderByDescending(r => r.RecordDate).ToList();

        if (recordList.Count == 0)
            throw new InvalidOperationException($"No medical records found for patient '{patientId}'.");

        var langName = ResolveLanguageName(language);

        var prompt = BuildPatientHistoryPrompt(patient, recordList, langName);

        return await CallGroqAsync(prompt, langName, ct);
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private string ResolveLanguageName(string input)
    {
        var match = Languages.FirstOrDefault(
            l => l.Code.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                 l.Name.Equals(input, StringComparison.OrdinalIgnoreCase));

        return match?.Name ?? input; // fall back to whatever the user typed
    }

    private static string BuildSingleRecordPrompt(Domain.Entities.MedicalRecord record, string language)
    {
        return $"""
            You are a professional medical report summarizer AI for a hospital management system.
            
            Summarize the following medical record in **{language}** language.
            
            Guidelines:
            - Write the summary in clear, patient-friendly language that anyone can understand.
            - Use simple terms; avoid complex medical jargon where possible, or explain it.
            - Structure the summary with these sections: Overview, Diagnosis, Treatment Plan, Medications, Lab Results (if any), and Follow-Up.
            - Be concise but thorough.
            - The ENTIRE response must be in {language}.
            
            === MEDICAL RECORD ===
            Record Date: {record.RecordDate:yyyy-MM-dd}
            Diagnosis: {record.Diagnosis}
            Treatment: {record.Treatment}
            Prescription: {record.Prescription ?? "N/A"}
            Lab Results: {record.LabResults ?? "N/A"}
            Notes: {record.Notes ?? "N/A"}
            Follow-Up Date: {(record.FollowUpDate.HasValue ? record.FollowUpDate.Value.ToString("yyyy-MM-dd") : "Not scheduled")}
            =======================
            """;
    }

    private static string BuildPatientHistoryPrompt(
        Domain.Entities.Patient patient,
        List<Domain.Entities.MedicalRecord> records,
        string language)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"""
            You are a professional medical report summarizer AI for a hospital management system.
            
            Summarize the complete medical history of the patient below in **{language}** language.
            
            Guidelines:
            - Write in clear, patient-friendly language.
            - Provide a high-level health timeline.
            - Highlight recurring diagnoses, ongoing treatments, and key lab trends.
            - Note any upcoming follow-ups.
            - Be concise but thorough.
            - The ENTIRE response must be in {language}.
            
            === PATIENT INFO ===
            Name: {patient.FirstName} {patient.LastName}
            Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}
            Gender: {patient.Gender}
            Blood Group: {patient.BloodGroup}
            =====================
            
            === MEDICAL RECORDS ({records.Count} total) ===
            """);

        foreach (var (r, idx) in records.Select((r, i) => (r, i)))
        {
            sb.AppendLine($"""
                
                --- Record #{idx + 1} ({r.RecordDate:yyyy-MM-dd}) ---
                Diagnosis: {r.Diagnosis}
                Treatment: {r.Treatment}
                Prescription: {r.Prescription ?? "N/A"}
                Lab Results: {r.LabResults ?? "N/A"}
                Notes: {r.Notes ?? "N/A"}
                Follow-Up: {(r.FollowUpDate.HasValue ? r.FollowUpDate.Value.ToString("yyyy-MM-dd") : "N/A")}
                """);
        }

        sb.AppendLine("=========================================");
        return sb.ToString();
    }

    private async Task<ReportSummaryResponseDto> CallGroqAsync(string prompt, string languageName, CancellationToken ct)
    {
        var requestBody = new
        {
            model = Model,
            messages = new[]
            {
                new { role = "system", content = $"You are a medical report summarizer. Always respond in {languageName}." },
                new { role = "user", content = prompt }
            },
            temperature = 0.3,
            max_tokens = 2048
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var request = new HttpRequestMessage(HttpMethod.Post, GroqApiUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Calling Groq API with model {Model} for language {Language}", Model, languageName);

        var response = await _httpClient.SendAsync(request, ct);
        var responseBody = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Groq API error {StatusCode}: {Body}", response.StatusCode, responseBody);
            throw new HttpRequestException($"Groq API returned {response.StatusCode}: {responseBody}");
        }

        var groqResponse = JsonSerializer.Deserialize<GroqChatResponse>(responseBody);

        var summary = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content ?? "No summary generated.";
        var tokensUsed = groqResponse?.Usage?.TotalTokens ?? 0;

        _logger.LogInformation("Groq API responded — {Tokens} tokens used", tokensUsed);

        return new ReportSummaryResponseDto
        {
            Summary = summary,
            Language = languageName,
            Model = Model,
            TokensUsed = tokensUsed,
            GeneratedAt = DateTime.UtcNow
        };
    }

    // ── Groq response models ─────────────────────────────────────────────────

    private sealed class GroqChatResponse
    {
        [JsonPropertyName("choices")]
        public List<GroqChoice>? Choices { get; set; }

        [JsonPropertyName("usage")]
        public GroqUsage? Usage { get; set; }
    }

    private sealed class GroqChoice
    {
        [JsonPropertyName("message")]
        public GroqMessage? Message { get; set; }
    }

    private sealed class GroqMessage
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }

    private sealed class GroqUsage
    {
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
