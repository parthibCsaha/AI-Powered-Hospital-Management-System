using HMS_Backend.Domain.Common;
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? ReasonForVisit { get; set; }
    public string? Notes { get; set; }
    public decimal? Fee { get; set; }
    public bool IsPaid { get; set; } = false;
    public string? CancellationReason { get; set; }
 
    // Navigation
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public MedicalRecord? MedicalRecord { get; set; }
    
}

