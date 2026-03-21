

namespace HMS_Backend.Application.DTOs.Appointment;


public class UpdateAppointmentDto
{
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public string? ReasonForVisit { get; set; }
    public string? Notes { get; set; }
    
}