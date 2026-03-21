

using Microsoft.EntityFrameworkCore.Storage;
using HMS_Backend.Application.Interfaces;
using HMS_Backend.Infrastructure.Persistence;
using HMS_Backend.Domain.Entities;
using HMS_Backend.Infrastructure.Persistence.Repositories;

namespace HMS_Backend.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private IRepository<Patient>? _patients;
    private IRepository<Doctor>? _doctors;
    private IRepository<Department>? _departments;
    private IRepository<Appointment>? _appointments;
    private IRepository<MedicalRecord>? _medicalRecords;
    private IRepository<DoctorSchedule>? _doctorSchedules;
    private IRepository<Room>? _rooms;
    private IRepository<Admission>? _admissions;
    private IRepository<User>? _users;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<Patient> Patients => _patients ??= new Repository<Patient>(_context);
    public IRepository<Doctor> Doctors => _doctors ??= new Repository<Doctor>(_context);
    public IRepository<Department> Departments => _departments ??= new Repository<Department>(_context);
    public IRepository<Appointment> Appointments => _appointments ??= new Repository<Appointment>(_context);
    public IRepository<MedicalRecord> MedicalRecords => _medicalRecords ??= new Repository<MedicalRecord>(_context);
    public IRepository<DoctorSchedule> DoctorSchedules => _doctorSchedules ??= new Repository<DoctorSchedule>(_context);
    public IRepository<Room> Rooms => _rooms ??= new Repository<Room>(_context);
    public IRepository<Admission> Admissions => _admissions ??= new Repository<Admission>(_context);
    public IRepository<User> Users => _users ??= new Repository<User>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
 
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
 
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }


}