




using AutoMapper;
using HMS_Backend.Application.DTOs.Department;
using HMS_Backend.Domain.Entities;
using HMS_Backend.Application.Interfaces;


namespace HMS_Backend.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
 
    public DepartmentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(CancellationToken ct = default)
    {
        var departments = await _uow.Departments.FindAsync(d => !d.IsDeleted, ct);
        return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments.OrderBy(d => d.Name));
    }
 
    public async Task<DepartmentResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var dept = await _uow.Departments.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct);
        return dept is null ? null : _mapper.Map<DepartmentResponseDto>(dept);
    }
 
    public async Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto, CancellationToken ct = default)
    {
        var nameExists = await _uow.Departments.AnyAsync(d => d.Name == dto.Name && !d.IsDeleted, ct);
        if (nameExists)
            throw new InvalidOperationException($"A department named '{dto.Name}' already exists.");
 
        var department = _mapper.Map<Department>(dto);
        await _uow.Departments.AddAsync(department, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<DepartmentResponseDto>(department);
    }
 
    public async Task<DepartmentResponseDto> UpdateAsync(Guid id, UpdateDepartmentDto dto, CancellationToken ct = default)
    {
        var department = await _uow.Departments.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Department with ID '{id}' not found.");
 
        var nameExists = await _uow.Departments.AnyAsync(d => d.Name == dto.Name && d.Id != id && !d.IsDeleted, ct);
        if (nameExists)
            throw new InvalidOperationException($"A department named '{dto.Name}' already exists.");
 
        _mapper.Map(dto, department);
        department.UpdatedAt = DateTime.UtcNow;
        _uow.Departments.Update(department);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<DepartmentResponseDto>(department);
    }
 
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var department = await _uow.Departments.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Department with ID '{id}' not found.");
 
        var hasDoctors = await _uow.Doctors.AnyAsync(d => d.DepartmentId == id && !d.IsDeleted, ct);
        if (hasDoctors)
            throw new InvalidOperationException("Cannot delete a department that has active doctors.");
 
        _uow.Departments.SoftDelete(department);
        await _uow.SaveChangesAsync(ct);
    }

}