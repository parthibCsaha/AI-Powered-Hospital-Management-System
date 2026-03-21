

using HMS_Backend.Application.DTOs;
using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Doctor;

namespace HMS_Backend.Application.Interfaces;

public interface IDoctorService
{
    
    Task<PagedResult<DoctorSummaryDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<DoctorResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<DoctorSummaryDto>> GetByDepartmentAsync(Guid departmentId, CancellationToken ct = default);
    Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto, CancellationToken ct = default);
    Task<DoctorResponseDto> UpdateAsync(Guid id, UpdateDoctorDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<DoctorScheduleDto>> GetSchedulesAsync(Guid doctorId, CancellationToken ct = default);
    Task UpsertScheduleAsync(Guid doctorId, DoctorScheduleDto dto, CancellationToken ct = default);

}