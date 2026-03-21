

using HMS_Backend.Application.DTOs.Department;



namespace HMS_Backend.Application.Interfaces;



public interface IDepartmentService
{
    Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(CancellationToken ct = default);
    Task<DepartmentResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto, CancellationToken ct = default);
    Task<DepartmentResponseDto> UpdateAsync(Guid id, UpdateDepartmentDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    
}