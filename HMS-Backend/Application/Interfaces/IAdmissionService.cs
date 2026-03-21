
using HMS_Backend.Application.DTOs.Admission;
using HMS_Backend.Application.Common;


namespace HMS_Backend.Application.Interfaces;


public interface IAdmissionService
{
    Task<PagedResult<AdmissionResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<AdmissionResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<AdmissionResponseDto>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);
    Task<AdmissionResponseDto> AdmitPatientAsync(CreateAdmissionDto dto, CancellationToken ct = default);
    Task<AdmissionResponseDto> DischargePatientAsync(Guid id, DischargePatientDto dto, CancellationToken ct = default);



}