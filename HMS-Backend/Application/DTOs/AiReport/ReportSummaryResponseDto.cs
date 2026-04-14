namespace HMS_Backend.Application.DTOs.AiReport;

public class ReportSummaryResponseDto
{
    public string Summary { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
