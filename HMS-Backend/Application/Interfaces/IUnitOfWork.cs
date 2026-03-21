using HMS_Backend.Domain.Entities;


namespace HMS_Backend.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Patient> Patients { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<Department> Departments { get; }
    IRepository<Appointment> Appointments { get; }
    IRepository<MedicalRecord> MedicalRecords { get; }
    IRepository<DoctorSchedule> DoctorSchedules { get; }
    IRepository<Room> Rooms { get; }
    IRepository<Admission> Admissions { get; }
    IRepository<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

}