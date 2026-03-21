

using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.MedicalRecord;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HMS_Backend.API.Controllers;

[Authorize]
[Route("api/v1/medical-records")]
[ApiController]
[Produces("application/json")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _service;
    public MedicalRecordsController(IMedicalRecordService service) => _service = service;
 
    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalRecordResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination, CancellationToken ct)
        => Ok(ApiResponse<PagedResult<MedicalRecordResponseDto>>.Ok(await _service.GetAllAsync(pagination, ct)));
 
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MedicalRecordResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null
            ? NotFound(ApiResponse.Fail($"Medical record '{id}' not found."))
            : Ok(ApiResponse<MedicalRecordResponseDto>.Ok(result));
    }
 
    [HttpGet("patient/{patientId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MedicalRecordResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPatient(Guid patientId, CancellationToken ct)
        => Ok(ApiResponse<IEnumerable<MedicalRecordResponseDto>>.Ok(await _service.GetByPatientAsync(patientId, ct)));
 
    [HttpPost]
    [Authorize(Roles = "Admin,Doctor")]
    [ProducesResponseType(typeof(ApiResponse<MedicalRecordResponseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<MedicalRecordResponseDto>.Ok(result, "Medical record created."));
    }
 
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Doctor")]
    [ProducesResponseType(typeof(ApiResponse<MedicalRecordResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMedicalRecordDto dto, CancellationToken ct)
        => Ok(ApiResponse<MedicalRecordResponseDto>.Ok(await _service.UpdateAsync(id, dto, ct), "Medical record updated."));
 
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return Ok(ApiResponse.Ok("Medical record deleted."));
    }
}