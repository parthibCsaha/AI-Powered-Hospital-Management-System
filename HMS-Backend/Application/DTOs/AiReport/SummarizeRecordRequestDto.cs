using System.ComponentModel.DataAnnotations;

namespace HMS_Backend.Application.DTOs.AiReport;

public class SummarizeRecordRequestDto
{
    /// <summary>
    /// ISO language code or language name, e.g. "en", "bn", "hi", "es", "fr", "ar", "zh" etc.
    /// </summary>
    [Required]
    public string Language { get; set; } = "en";
}

public class SummarizePatientHistoryRequestDto
{
    /// <summary>
    /// ISO language code or language name, e.g. "en", "bn", "hi", "es", "fr", "ar", "zh" etc.
    /// </summary>
    [Required]
    public string Language { get; set; } = "en";
}
