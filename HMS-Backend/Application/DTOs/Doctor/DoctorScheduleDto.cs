
namespace HMS_Backend.Application.DTOs.Doctor;


public class DoctorScheduleDto
{
    public Guid DoctorId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 30;
    public bool IsAvailable { get; set; } = true;

}