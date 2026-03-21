

using AutoMapper;
using HMS_Backend.Application.DTOs.Admission;
using HMS_Backend.Application.Common;
using HMS_Backend.Domain.Entities;
using HMS_Backend.Application.Interfaces;

namespace HMS_Backend.Application.Services;


public class AdmissionService : IAdmissionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
 
    public AdmissionService(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }
 
    public async Task<PagedResult<AdmissionResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var all = await _uow.Admissions.FindAsync(a => !a.IsDeleted, ct);
        var ordered = all.OrderByDescending(a => a.AdmissionDate);
        var total = ordered.Count();
        var items = ordered.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
        return PagedResult<AdmissionResponseDto>.Create(_mapper.Map<IEnumerable<AdmissionResponseDto>>(items), total, pagination.PageNumber, pagination.PageSize);
    }
 
    public async Task<AdmissionResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var admission = await _uow.Admissions.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
        return admission is null ? null : _mapper.Map<AdmissionResponseDto>(admission);
    }
 
    public async Task<IEnumerable<AdmissionResponseDto>> GetByPatientAsync(Guid patientId, CancellationToken ct = default)
    {
        var admissions = await _uow.Admissions.FindAsync(a => a.PatientId == patientId && !a.IsDeleted, ct);
        return _mapper.Map<IEnumerable<AdmissionResponseDto>>(admissions.OrderByDescending(a => a.AdmissionDate));
    }
 
    public async Task<AdmissionResponseDto> AdmitPatientAsync(CreateAdmissionDto dto, CancellationToken ct = default)
    {
        if (!await _uow.Patients.AnyAsync(p => p.Id == dto.PatientId && !p.IsDeleted, ct))
            throw new KeyNotFoundException($"Patient '{dto.PatientId}' not found.");
        if (!await _uow.Doctors.AnyAsync(d => d.Id == dto.DoctorId && !d.IsDeleted, ct))
            throw new KeyNotFoundException($"Doctor '{dto.DoctorId}' not found.");
 
        var room = await _uow.Rooms.FirstOrDefaultAsync(r => r.Id == dto.RoomId && !r.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Room '{dto.RoomId}' not found.");
        if (room.IsOccupied)
            throw new InvalidOperationException($"Room '{room.RoomNumber}' is already occupied.");
 
        var activeAdmission = await _uow.Admissions.AnyAsync(
            a => a.PatientId == dto.PatientId && !a.IsDischarged && !a.IsDeleted, ct);
        if (activeAdmission)
            throw new InvalidOperationException("Patient is already admitted.");
 
        var admission = _mapper.Map<Admission>(dto);
        await _uow.Admissions.AddAsync(admission, ct);
 
        room.IsOccupied = true;
        _uow.Rooms.Update(room);
 
        await _uow.SaveChangesAsync(ct);
        var created = await _uow.Admissions.FirstOrDefaultAsync(a => a.Id == admission.Id, ct);
        return _mapper.Map<AdmissionResponseDto>(created!);
    }
 
    public async Task<AdmissionResponseDto> DischargePatientAsync(Guid id, DischargePatientDto dto, CancellationToken ct = default)
    {
        var admission = await _uow.Admissions.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Admission '{id}' not found.");
        if (admission.IsDischarged)
            throw new InvalidOperationException("Patient has already been discharged.");
 
        admission.DischargeDate = DateTime.UtcNow;
        admission.DischargeNotes = dto.DischargeNotes;
        admission.IsDischarged = true;
        admission.UpdatedAt = DateTime.UtcNow;
 
        var room = await _uow.Rooms.FirstOrDefaultAsync(r => r.Id == admission.RoomId, ct);
        if (room is not null)
        {
            var days = (int)(DateTime.UtcNow - admission.AdmissionDate).TotalDays;
            admission.TotalBill = Math.Max(1, days) * room.DailyRate;
            room.IsOccupied = false;
            _uow.Rooms.Update(room);
        }
 
        _uow.Admissions.Update(admission);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<AdmissionResponseDto>(admission);
    }
}