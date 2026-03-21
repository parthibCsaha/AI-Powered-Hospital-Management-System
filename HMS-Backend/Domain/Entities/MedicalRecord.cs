using HMS_Backend.Domain.Common;

namespace HMS_Backend.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Prescription { get; set; }
    public string? LabResults { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; } = DateTime.UtcNow;
    public DateTime? FollowUpDate { get; set; }
 
    // Navigation
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public Appointment? Appointment { get; set; }
    
}