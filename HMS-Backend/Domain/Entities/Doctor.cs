using HMS_Backend.Domain.Common;
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Domain.Entities;

public class Doctor : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"Dr. {FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public Gender Gender { get; set; }
    public decimal ConsultationFee { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? Qualifications { get; set; }
    public string? Biography { get; set; }
 
    // Foreign Key
    public Guid DepartmentId { get; set; }
 
    // Navigation
    public Department Department { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    

}