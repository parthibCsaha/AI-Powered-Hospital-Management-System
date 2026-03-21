
namespace HMS_Backend.Application.DTOs.Appointment;

public class CreateAppointmentDto
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public string? ReasonForVisit { get; set; }

}

