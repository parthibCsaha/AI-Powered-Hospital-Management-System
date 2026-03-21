using HMS_Backend.Domain.Common;

namespace HMS_Backend.Domain.Entities;


public class DoctorSchedule : BaseEntity
{
    public Guid DoctorId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotDurationMinutes { get; set; } = 30;
    public bool IsAvailable { get; set; } = true;
 
    // Navigation
    public Doctor Doctor { get; set; } = null!;
    
}