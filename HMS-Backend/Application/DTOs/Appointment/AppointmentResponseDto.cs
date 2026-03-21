
namespace HMS_Backend.Application.DTOs.Appointment;

public class AppointmentResponseDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorSpecialization { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ReasonForVisit { get; set; }
    public string? Notes { get; set; }
    public decimal? Fee { get; set; }
    public bool IsPaid { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; }
    
}
