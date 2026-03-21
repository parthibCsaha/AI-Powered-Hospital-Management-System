
namespace HMS_Backend.Application.DTOs.Doctor;



public class UpdateDoctorDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public decimal ConsultationFee { get; set; }
    public bool IsAvailable { get; set; }
    public Guid DepartmentId { get; set; }
    public string? Qualifications { get; set; }
    public string? Biography { get; set; }

}