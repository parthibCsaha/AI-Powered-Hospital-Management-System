
namespace HMS_Backend.Application.DTOs.Doctor;


public class DoctorSummaryDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public bool IsAvailable { get; set; }

}
 