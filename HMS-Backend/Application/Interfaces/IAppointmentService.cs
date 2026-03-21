

using HMS_Backend.Application.DTOs.Appointment;
using HMS_Backend.Application.Common;

namespace HMS_Backend.Application.Interfaces;

public interface IAppointmentService
{
    Task<PagedResult<AppointmentResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default);
    Task<AppointmentResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(Guid patientId, CancellationToken ct = default);
    Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(Guid doctorId, DateTime date, CancellationToken ct = default);
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto, CancellationToken ct = default);
    Task<AppointmentResponseDto> UpdateAsync(Guid id, UpdateAppointmentDto dto, CancellationToken ct = default);
    Task UpdateStatusAsync(Guid id, UpdateAppointmentStatusDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);

    
}