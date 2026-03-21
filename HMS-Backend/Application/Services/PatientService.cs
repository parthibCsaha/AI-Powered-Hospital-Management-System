
using AutoMapper;
using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Patient;
using HMS_Backend.Application.Interfaces;
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Application.Services;


public class PatientService : IPatientService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PatientService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<PatientSummaryDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
    {
        var all = await _uow.Patients.FindAsync(p => !p.IsDeleted, ct);

        if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
        {
            var term = pagination.SearchTerm.ToLower();
            all = all.Where(p =>
                p.FirstName.ToLower().Contains(term) ||
                p.LastName.ToLower().Contains(term) ||
                p.Email.ToLower().Contains(term) ||
                p.PhoneNumber.ToLower().Contains(term)
            );
        }

        var ordered = pagination.SortDescending ? all.OrderByDescending(p => p.CreatedAt) : all.OrderBy(p => p.LastName);

        var totalCount = ordered.Count();

        var items = ordered
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        return PagedResult<PatientSummaryDto>.Create(
            _mapper.Map<List<PatientSummaryDto>>(items),
            totalCount,
            pagination.PageNumber,
            pagination.PageSize
        );

    }

    public async Task<PatientResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await _uow.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);
        if (patient == null) return null;
        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> CreateAsync(CreatePatientDto dto, CancellationToken ct = default)
    {
        var exists = await _uow.Patients.AnyAsync(p => p.Email == dto.Email.ToLower() && !p.IsDeleted, ct);

        if (exists) throw new InvalidOperationException("A patient with the same email already exists.");

        var patient = _mapper.Map<Patient>(dto);

        await _uow.Patients.AddAsync(patient, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<PatientResponseDto>(patient);

    }

    public async Task<PatientResponseDto> UpdateAsync(Guid id, UpdatePatientDto dto, CancellationToken ct = default)
    {
        var patient = await _uow.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);

        if (patient == null) throw new KeyNotFoundException($"Patient with id {id} not found.");

        var emailTaken = await _uow.Patients.AnyAsync(p => p.Email == dto.Email && p.Id != id && !p.IsDeleted, ct);

        if (emailTaken) throw new InvalidOperationException("Another patient with the same email already exists.");

        _mapper.Map(dto, patient);

        patient.UpdatedAt = DateTime.UtcNow;

        _uow.Patients.Update(patient);

        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<PatientResponseDto>(patient);

    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await _uow.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Patient with ID '{id}' not found.");
 
        _uow.Patients.SoftDelete(patient);
        await _uow.SaveChangesAsync(ct);
    }


    
}