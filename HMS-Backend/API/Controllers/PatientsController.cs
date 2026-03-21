

using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Patient;
using Microsoft.AspNetCore.Mvc;


namespace HMS_Backend.API.Controllers;


[Authorize]
public class PatientsController : BaseApiController
{
    private readonly IPatientService _patientService;
    public PatientsController(IPatientService patientService)
       => _patientService = patientService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PatientSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        var result = await _patientService.GetAllAsync(pagination, cancellationToken);
        return Ok(ApiResponse<PagedResult<PatientSummaryDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _patientService.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse.Fail($"Patient with ID '{id}' not found."));
        return Ok(ApiResponse<PatientResponseDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePatientDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _patientService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<PatientResponseDto>.Ok(result, "Patient created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<PatientResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePatientDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _patientService.UpdateAsync(id, dto, cancellationToken);
        return Ok(ApiResponse<PatientResponseDto>.Ok(result, "Patient updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _patientService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse.Ok("Patient deleted successfully."));
    }
    

}