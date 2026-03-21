
using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Patient;

namespace HMS_Backend.Application.Interfaces;

public interface IPatientService
{
    Task<PagedResult<PatientSummaryDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<PatientResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PatientResponseDto> CreateAsync(CreatePatientDto dto, CancellationToken ct = default);
    Task<PatientResponseDto> UpdateAsync(Guid id, UpdatePatientDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);


}