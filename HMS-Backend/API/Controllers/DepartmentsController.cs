

using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Department;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Backend.API.Controllers;


[Authorize]
public class DepartmentsController : BaseApiController
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
        => _departmentService = departmentService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IEnumerable<DepartmentResponseDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DepartmentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse.Fail($"Department with ID '{id}' not found."));
        return Ok(ApiResponse<DepartmentResponseDto>.Ok(result));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<DepartmentResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<DepartmentResponseDto>.Ok(result, "Department created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<DepartmentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDepartmentDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _departmentService.UpdateAsync(id, dto, cancellationToken);
        return Ok(ApiResponse<DepartmentResponseDto>.Ok(result, "Department updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _departmentService.DeleteAsync(id, cancellationToken);
        return Ok(ApiResponse.Ok("Department deleted successfully."));
    }

}