using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.AiReport;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Backend.API.Controllers;

[Authorize]
[Route("api/v1/ai-reports")]
[ApiController]
[Produces("application/json")]
public class AiReportsController : ControllerBase
{
    private readonly IAiReportService _aiService;
    public AiReportsController(IAiReportService aiService) => _aiService = aiService;

    /// <summary>
    /// Lists all supported languages for AI report summarization.
    /// </summary>
    [HttpGet("languages")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<SupportedLanguageDto>>), StatusCodes.Status200OK)]
    public IActionResult GetLanguages()
        => Ok(ApiResponse<IReadOnlyList<SupportedLanguageDto>>.Ok(_aiService.GetSupportedLanguages()));

    /// <summary>
    /// Summarizes a single medical record in the chosen language using AI.
    /// </summary>
    [HttpPost("summarize/record/{recordId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ReportSummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SummarizeRecord(
        Guid recordId,
        [FromBody] SummarizeRecordRequestDto dto,
        CancellationToken ct)
    {
        var result = await _aiService.SummarizeRecordAsync(recordId, dto.Language, ct);
        return Ok(ApiResponse<ReportSummaryResponseDto>.Ok(result, "Report summarized successfully."));
    }

    /// <summary>
    /// Summarizes all medical records for a patient in the chosen language using AI.
    /// </summary>
    [HttpPost("summarize/patient/{patientId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ReportSummaryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SummarizePatientHistory(
        Guid patientId,
        [FromBody] SummarizePatientHistoryRequestDto dto,
        CancellationToken ct)
    {
        var result = await _aiService.SummarizePatientHistoryAsync(patientId, dto.Language, ct);
        return Ok(ApiResponse<ReportSummaryResponseDto>.Ok(result, "Patient history summarized successfully."));
    }
}
