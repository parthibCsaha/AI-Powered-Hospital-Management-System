using HMS_Backend.Application.DTOs.AiReport;

namespace HMS_Backend.Application.Interfaces;

public interface IAiReportService
{
    /// <summary>
    /// Summarizes a medical record by its ID in the requested language.
    /// </summary>
    Task<ReportSummaryResponseDto> SummarizeRecordAsync(Guid medicalRecordId, string language, CancellationToken ct = default);

    /// <summary>
    /// Summarizes all medical records for a patient in the requested language.
    /// </summary>
    Task<ReportSummaryResponseDto> SummarizePatientHistoryAsync(Guid patientId, string language, CancellationToken ct = default);

    /// <summary>
    /// Returns the list of supported languages.
    /// </summary>
    IReadOnlyList<SupportedLanguageDto> GetSupportedLanguages();
}
