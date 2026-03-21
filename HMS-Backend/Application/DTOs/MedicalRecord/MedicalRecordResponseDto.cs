

namespace HMS_Backend.Application.DTOs.MedicalRecord;


public class MedicalRecordResponseDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public Guid? AppointmentId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Prescription { get; set; }
    public string? LabResults { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
    public DateTime? FollowUpDate { get; set; }
    
}