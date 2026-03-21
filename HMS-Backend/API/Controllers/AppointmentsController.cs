
using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Appointment;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Backend.API.Controllers;


[Authorize]
public class AppointmentsController : BaseApiController
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
        => _appointmentService = appointmentService;

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        var result = await _appointmentService.GetAllAsync(pagination, cancellationToken);
        return Ok(ApiResponse<PagedResult<AppointmentResponseDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _appointmentService.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse.Fail($"Appointment with ID '{id}' not found."));
        return Ok(ApiResponse<AppointmentResponseDto>.Ok(result));
    }

    [HttpGet("patient/{patientId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPatient(Guid patientId, CancellationToken cancellationToken)
    {
        var result = await _appointmentService.GetByPatientAsync(patientId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<AppointmentResponseDto>>.Ok(result));
    }

    [HttpGet("doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDoctor(
        Guid doctorId,
        [FromQuery] DateTime date,
        CancellationToken cancellationToken)
    {
        var result = await _appointmentService.GetByDoctorAsync(doctorId, date, cancellationToken);
        return Ok(ApiResponse<IEnumerable<AppointmentResponseDto>>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist,Patient")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _appointmentService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<AppointmentResponseDto>.Ok(result, "Appointment booked successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _appointmentService.UpdateAsync(id, dto, cancellationToken);
        return Ok(ApiResponse<AppointmentResponseDto>.Ok(result, "Appointment updated successfully."));
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Doctor,Receptionist")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateAppointmentStatusDto dto,
        CancellationToken cancellationToken)
    {
        await _appointmentService.UpdateStatusAsync(id, dto, cancellationToken);
        return Ok(ApiResponse.Ok("Appointment status updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _appointmentService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse.Ok("Appointment deleted successfully."));
    }

}