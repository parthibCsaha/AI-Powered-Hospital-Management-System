using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Admission;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Backend.API.Controllers;

[Authorize]
[Route("api/v1/admissions")]
[ApiController]
[Produces("application/json")]
public class AdmissionsController : ControllerBase
{
    private readonly IAdmissionService _service;
    public AdmissionsController(IAdmissionService service) => _service = service;
 
    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AdmissionResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination, CancellationToken ct)
        => Ok(ApiResponse<PagedResult<AdmissionResponseDto>>.Ok(await _service.GetAllAsync(pagination, ct)));
 
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdmissionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound(ApiResponse.Fail($"Admission '{id}' not found.")) : Ok(ApiResponse<AdmissionResponseDto>.Ok(result));
    }
 
    [HttpGet("patient/{patientId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AdmissionResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPatient(Guid patientId, CancellationToken ct)
        => Ok(ApiResponse<IEnumerable<AdmissionResponseDto>>.Ok(await _service.GetByPatientAsync(patientId, ct)));
 
    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<AdmissionResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Admit([FromBody] CreateAdmissionDto dto, CancellationToken ct)
    {
        var result = await _service.AdmitPatientAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<AdmissionResponseDto>.Ok(result, "Patient admitted successfully."));
    }
 
    [HttpPatch("{id:guid}/discharge")]
    [Authorize(Roles = "Admin,Doctor,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<AdmissionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Discharge(Guid id, [FromBody] DischargePatientDto dto, CancellationToken ct)
        => Ok(ApiResponse<AdmissionResponseDto>.Ok(await _service.DischargePatientAsync(id, dto, ct), "Patient discharged successfully."));
}