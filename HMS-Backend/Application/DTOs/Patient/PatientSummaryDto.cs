

namespace HMS_Backend.Application.DTOs.Patient;

public class PatientSummaryDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
}