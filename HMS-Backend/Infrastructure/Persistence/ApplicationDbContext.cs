
using HMS_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HMS_Backend.Infrastructure.Persistence.Configurations;


namespace HMS_Backend.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Admission> Admissions => Set<Admission>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PatientConfiguration());
        modelBuilder.ApplyConfiguration(new DoctorConfiguration());
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
        modelBuilder.ApplyConfiguration(new MedicalRecordConfiguration());
        modelBuilder.ApplyConfiguration(new DoctorScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new AdmissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Doctor>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<MedicalRecord>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<DoctorSchedule>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Room>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Admission>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);


    }

}

