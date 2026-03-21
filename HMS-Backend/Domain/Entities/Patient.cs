using HMS_Backend.Domain.Common;
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Domain.Entities;

public class Patient : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public int Age
    {
        get
        {
            var today = DateTime.UtcNow.Date;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Allergies { get; set; }
    public string? ChronicConditions { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    

}