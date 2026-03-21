

using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HMS_Backend.Application.Interfaces;

namespace HMS_Backend.API.Controllers;

public class DoctorsController : BaseApiController
{
    private readonly IDoctorService _doctorService;
 
    public DoctorsController(IDoctorService doctorService)
        => _doctorService = doctorService;


    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DoctorSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
    {
        var result = await _doctorService.GetAllAsync(pagination, cancellationToken);
        return Ok(ApiResponse<PagedResult<DoctorSummaryDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DoctorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _doctorService.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse.Fail($"Doctor with ID '{id}' not found."));
        return Ok(ApiResponse<DoctorResponseDto>.Ok(result));
    }

    [HttpGet("by-department/{departmentId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<DoctorSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDepartment(Guid departmentId, CancellationToken cancellationToken)
    {
        var result = await _doctorService.GetByDepartmentAsync(departmentId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<DoctorSummaryDto>>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<DoctorResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto, CancellationToken cancellationToken)
    {
        var result = await _doctorService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<DoctorResponseDto>.Ok(result, "Doctor registered successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<DoctorResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDoctorDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _doctorService.UpdateAsync(id, dto, cancellationToken);
        return Ok(ApiResponse<DoctorResponseDto>.Ok(result, "Doctor updated successfully."));
    }
 
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _doctorService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse.Ok("Doctor deleted successfully."));
    }
 
    [HttpGet("{id:guid}/schedules")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<DoctorScheduleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedules(Guid id, CancellationToken cancellationToken)
    {
        var result = await _doctorService.GetSchedulesAsync(id, cancellationToken);
        return Ok(ApiResponse<IEnumerable<DoctorScheduleDto>>.Ok(result));
    }
 
    [HttpPut("{id:guid}/schedules")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpsertSchedule(
        Guid id,
        [FromBody] DoctorScheduleDto dto,
        CancellationToken cancellationToken)
    {
        await _doctorService.UpsertScheduleAsync(id, dto, cancellationToken);
        return Ok(ApiResponse.Ok("Doctor schedule updated successfully."));
    }
    
}