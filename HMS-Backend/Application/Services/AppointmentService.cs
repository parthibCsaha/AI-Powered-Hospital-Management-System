

using AutoMapper;
using HMS_Backend.Application.DTOs.Appointment;
using HMS_Backend.Application.Common;
using HMS_Backend.Domain.Entities;
using HMS_Backend.Domain.Enums;
using HMS_Backend.Application.Interfaces;

namespace HMS_Backend.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public AppointmentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<AppointmentResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var all = await _uow.Appointments.FindAsync(a => !a.IsDeleted, ct);

        if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
        {
            var term = pagination.SearchTerm.ToLower();
            all = all.Where(a =>
                (a.Patient != null && a.Patient.FullName.ToLower().Contains(term)) ||
                (a.Doctor != null && a.Doctor.FullName.ToLower().Contains(term)));
        }
 
        var ordered = all.OrderByDescending(a => a.AppointmentDate).ThenBy(a => a.StartTime);
        var totalCount = ordered.Count();
        var items = ordered
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();
 
        return PagedResult<AppointmentResponseDto>.Create(
            _mapper.Map<IEnumerable<AppointmentResponseDto>>(items),
            totalCount, pagination.PageNumber, pagination.PageSize);

    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var appt = await _uow.Appointments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
        return appt is null ? null : _mapper.Map<AppointmentResponseDto>(appt);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(Guid patientId, CancellationToken ct = default)
    {
        var appts = await _uow.Appointments.FindAsync(a => a.PatientId == patientId && !a.IsDeleted, ct);
        return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appts.OrderByDescending(a => a.AppointmentDate));
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByDoctorAsync(Guid doctorId, DateTime date, CancellationToken ct = default)
    {
        var appts = await _uow.Appointments.FindAsync(
            a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date && !a.IsDeleted, ct);
        return _mapper.Map<IEnumerable<AppointmentResponseDto>>(appts.OrderBy(a => a.StartTime));
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto, CancellationToken ct = default)
    {
        var patientExists = await _uow.Patients.AnyAsync(p => p.Id == dto.PatientId && !p.IsDeleted, ct);

        if (!patientExists)
            throw new KeyNotFoundException($"Patient with ID '{dto.PatientId}' not found.");

        var doctor = await _uow.Doctors.FirstOrDefaultAsync(d => d.Id == dto.DoctorId && !d.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Doctor with ID '{dto.DoctorId}' not found.");   

        if (!doctor.IsAvailable) 
            throw new InvalidOperationException($"Doctor '{doctor.FullName}' is currently unavailable for appointments.");
        
        var endTime = dto.StartTime.Add(TimeSpan.FromMinutes(30));

        var conflict = await _uow.Appointments.AnyAsync(a =>
            a.DoctorId == dto.DoctorId &&
            a.AppointmentDate.Date == dto.AppointmentDate.Date &&
            a.Status != AppointmentStatus.Cancelled &&
            !a.IsDeleted &&
            a.StartTime < endTime && a.EndTime > dto.StartTime, ct);

        if (conflict)
            throw new InvalidOperationException("The doctor has another appointment during the requested time slot.");

        var appointment = _mapper.Map<Appointment>(dto);
        appointment.EndTime = endTime;
        appointment.Fee = doctor.ConsultationFee;

        await _uow.Appointments.AddAsync(appointment, ct);
        await _uow.SaveChangesAsync(ct);

        var created = await _uow.Appointments.FirstOrDefaultAsync(a => a.Id == appointment.Id, ct);
        return _mapper.Map<AppointmentResponseDto>(created!);

    }

    public async Task<AppointmentResponseDto> UpdateAsync(Guid id, UpdateAppointmentDto dto, CancellationToken ct = default)
    {
        var appointment = await _uow.Appointments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Appointment with ID '{id}' not found.");
 
        if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("Cannot update a completed or cancelled appointment.");
 
        _mapper.Map(dto, appointment);
        appointment.UpdatedAt = DateTime.UtcNow;
        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync(ct);
 
        return _mapper.Map<AppointmentResponseDto>(appointment);
    }

    public async Task UpdateStatusAsync(Guid id, UpdateAppointmentStatusDto dto, CancellationToken ct = default)
    {
        var appointment = await _uow.Appointments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Appointment with ID '{id}' not found.");
 
        if (dto.Status == AppointmentStatus.Cancelled && string.IsNullOrWhiteSpace(dto.CancellationReason))
            throw new InvalidOperationException("Cancellation reason is required when cancelling an appointment.");
 
        appointment.Status = dto.Status;
        appointment.CancellationReason = dto.CancellationReason;
        appointment.UpdatedAt = DateTime.UtcNow;
        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var appointment = await _uow.Appointments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Appointment with ID '{id}' not found.");
 
        _uow.Appointments.SoftDelete(appointment);
        await _uow.SaveChangesAsync(ct);
    }


}