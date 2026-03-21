
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Application.DTOs.Doctor;


 
public class CreateDoctorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public Gender Gender { get; set; }
    public decimal ConsultationFee { get; set; }
    public Guid DepartmentId { get; set; }
    public string? Qualifications { get; set; }
    public string? Biography { get; set; }

}