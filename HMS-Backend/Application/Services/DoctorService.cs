
using AutoMapper;
using HMS_Backend.Application.DTOs.Doctor;
using HMS_Backend.Application.Common;
using HMS_Backend.Application.Interfaces;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Application.Services;



public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public DoctorService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<DoctorSummaryDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var all = await _uow.Doctors.FindAsync(d => !d.IsDeleted, ct);
 
        if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
        {
            var term = pagination.SearchTerm.ToLower();
            all = all.Where(d =>
                d.FirstName.ToLower().Contains(term) ||
                d.LastName.ToLower().Contains(term) ||
                d.Specialization.ToLower().Contains(term));
        }
 
        var ordered = pagination.SortDescending
            ? all.OrderByDescending(d => d.CreatedAt)
            : all.OrderBy(d => d.LastName);
 
        var totalCount = ordered.Count();
        var items = ordered
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();
 
        return PagedResult<DoctorSummaryDto>.Create(
            _mapper.Map<IEnumerable<DoctorSummaryDto>>(items),
            totalCount,
            pagination.PageNumber,
            pagination.PageSize);
    }

    public async Task<DoctorResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await _uow.Doctors.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct);
        return doctor is null ? null : _mapper.Map<DoctorResponseDto>(doctor);
    }
 
    public async Task<IEnumerable<DoctorSummaryDto>> GetByDepartmentAsync(Guid departmentId, CancellationToken ct = default)
    {
        var doctors = await _uow.Doctors.FindAsync(d => d.DepartmentId == departmentId && !d.IsDeleted, ct);
        return _mapper.Map<IEnumerable<DoctorSummaryDto>>(doctors);
    }

    public async Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto, CancellationToken ct = default)
    {
        var deptExists = await _uow.Departments.AnyAsync(d => d.Id == dto.DepartmentId && !d.IsDeleted, ct);
        if (!deptExists)
            throw new KeyNotFoundException($"Department with ID '{dto.DepartmentId}' not found.");
 
        var emailExists = await _uow.Doctors.AnyAsync(d => d.Email == dto.Email && !d.IsDeleted, ct);
        if (emailExists)
            throw new InvalidOperationException($"A doctor with email '{dto.Email}' already exists.");
 
        var licenseExists = await _uow.Doctors.AnyAsync(d => d.LicenseNumber == dto.LicenseNumber && !d.IsDeleted, ct);
        if (licenseExists)
            throw new InvalidOperationException($"A doctor with license number '{dto.LicenseNumber}' already exists.");
 
        var doctor = _mapper.Map<Doctor>(dto);
        await _uow.Doctors.AddAsync(doctor, ct);
        await _uow.SaveChangesAsync(ct);
 
        var created = await _uow.Doctors.FirstOrDefaultAsync(d => d.Id == doctor.Id, ct);
        return _mapper.Map<DoctorResponseDto>(created!);
    }

    public async Task<DoctorResponseDto> UpdateAsync(Guid id, UpdateDoctorDto dto, CancellationToken ct = default)
    {
        var doctor = await _uow.Doctors.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Doctor with ID '{id}' not found.");
 
        var deptExists = await _uow.Departments.AnyAsync(d => d.Id == dto.DepartmentId && !d.IsDeleted, ct);
        if (!deptExists)
            throw new KeyNotFoundException($"Department with ID '{dto.DepartmentId}' not found.");
 
        _mapper.Map(dto, doctor);
        doctor.UpdatedAt = DateTime.UtcNow;
        _uow.Doctors.Update(doctor);
        await _uow.SaveChangesAsync(ct);
 
        return _mapper.Map<DoctorResponseDto>(doctor);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await _uow.Doctors.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Doctor with ID '{id}' not found.");
 
        _uow.Doctors.SoftDelete(doctor);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<DoctorScheduleDto>> GetSchedulesAsync(Guid doctorId, CancellationToken ct = default)
    {
        var shedules = await _uow.DoctorSchedules.FindAsync(s => s.DoctorId == doctorId && !s.IsDeleted, ct);

        return _mapper.Map<IEnumerable<DoctorScheduleDto>>(shedules.OrderBy(s => s.DayOfWeek));

    }

    public async Task UpsertScheduleAsync(Guid doctorId, DoctorScheduleDto dto, CancellationToken ct = default)
    {
        var doctorExists = await _uow.Doctors.AnyAsync(d => d.Id == doctorId && !d.IsDeleted, ct);
        if (!doctorExists)
            throw new KeyNotFoundException($"Doctor with ID '{doctorId}' not found.");
 
        if (dto.EndTime <= dto.StartTime)
            throw new InvalidOperationException("End time must be after start time.");
 
        var existing = await _uow.DoctorSchedules.FirstOrDefaultAsync(
            s => s.DoctorId == doctorId && s.DayOfWeek == dto.DayOfWeek && !s.IsDeleted, ct);
 
        if (existing is null)
        {
            var schedule = _mapper.Map<DoctorSchedule>(dto);
            schedule.DoctorId = doctorId;
            await _uow.DoctorSchedules.AddAsync(schedule, ct);
        }
        else
        {
            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;
            existing.SlotDurationMinutes = dto.SlotDurationMinutes;
            existing.IsAvailable = dto.IsAvailable;
            existing.UpdatedAt = DateTime.UtcNow;
            _uow.DoctorSchedules.Update(existing);
        }
 
        await _uow.SaveChangesAsync(ct);
    }

}