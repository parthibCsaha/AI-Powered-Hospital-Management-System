
namespace HMS_Backend.Application.DTOs.Doctor;


public class DoctorResponseDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string Gender { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public bool IsAvailable { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Qualifications { get; set; }
    public string? Biography { get; set; }
    public DateTime CreatedAt { get; set; }

}