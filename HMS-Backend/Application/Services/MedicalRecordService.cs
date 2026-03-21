
using AutoMapper;
using HMS_Backend.Application.DTOs.MedicalRecord;
using HMS_Backend.Application.Common;
using HMS_Backend.Domain.Entities;
using HMS_Backend.Application.Interfaces;

namespace HMS_Backend.Application.Services;



public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
 
    public MedicalRecordService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
 
    public async Task<PagedResult<MedicalRecordResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var all = await _uow.MedicalRecords.FindAsync(r => !r.IsDeleted, ct);
        
        var ordered = all.OrderByDescending(r => r.RecordDate);
        
        var total = ordered.Count();
        
        var items = ordered
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();
        
        return PagedResult<MedicalRecordResponseDto>.Create(
            _mapper.Map<IEnumerable<MedicalRecordResponseDto>>(items),
            total, 
            pagination.PageNumber, 
            pagination.PageSize
        );
    }
 
    public async Task<MedicalRecordResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var record = await _uow.MedicalRecords.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
        return record is null ? null : _mapper.Map<MedicalRecordResponseDto>(record);
    }
 
    public async Task<IEnumerable<MedicalRecordResponseDto>> GetByPatientAsync(Guid patientId, CancellationToken ct = default)
    {
        var records = await _uow.MedicalRecords.FindAsync(r => r.PatientId == patientId && !r.IsDeleted, ct);
        return _mapper.Map<IEnumerable<MedicalRecordResponseDto>>(records.OrderByDescending(r => r.RecordDate));
    }
 
    public async Task<MedicalRecordResponseDto> CreateAsync(CreateMedicalRecordDto dto, CancellationToken ct = default)
    {
        if (!await _uow.Patients.AnyAsync(p => p.Id == dto.PatientId && !p.IsDeleted, ct))
            throw new KeyNotFoundException($"Patient '{dto.PatientId}' not found.");

        if (!await _uow.Doctors.AnyAsync(d => d.Id == dto.DoctorId && !d.IsDeleted, ct))
            throw new KeyNotFoundException($"Doctor '{dto.DoctorId}' not found.");
 
        var record = _mapper.Map<MedicalRecord>(dto);
        await _uow.MedicalRecords.AddAsync(record, ct);
        await _uow.SaveChangesAsync(ct);
        var created = await _uow.MedicalRecords.FirstOrDefaultAsync(r => r.Id == record.Id, ct);
        return _mapper.Map<MedicalRecordResponseDto>(created!);
    }
 
    public async Task<MedicalRecordResponseDto> UpdateAsync(Guid id, UpdateMedicalRecordDto dto, CancellationToken ct = default)
    {
        var record = await _uow.MedicalRecords.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Medical record '{id}' not found.");
 
        _mapper.Map(dto, record);
        record.UpdatedAt = DateTime.UtcNow;
        _uow.MedicalRecords.Update(record);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<MedicalRecordResponseDto>(record);
    }
 
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var record = await _uow.MedicalRecords.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Medical record '{id}' not found.");
            
        _uow.MedicalRecords.SoftDelete(record);
        await _uow.SaveChangesAsync(ct);
    }
}