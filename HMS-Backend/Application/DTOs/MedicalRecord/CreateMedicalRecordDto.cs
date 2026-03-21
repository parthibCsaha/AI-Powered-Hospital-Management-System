
namespace HMS_Backend.Application.DTOs.MedicalRecord;


public class CreateMedicalRecordDto
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Prescription { get; set; }
    public string? LabResults { get; set; }
    public string? Notes { get; set; }
    public DateTime? FollowUpDate { get; set; }

}