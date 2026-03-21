
using HMS_Backend.Application.DTOs.MedicalRecord;
using HMS_Backend.Application.Common;


namespace HMS_Backend.Application.Interfaces;

public interface IMedicalRecordService
{
    Task<PagedResult<MedicalRecordResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<MedicalRecordResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<MedicalRecordResponseDto>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);
    Task<MedicalRecordResponseDto> CreateAsync(CreateMedicalRecordDto dto, CancellationToken ct = default);
    Task<MedicalRecordResponseDto> UpdateAsync(Guid id, UpdateMedicalRecordDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);


}