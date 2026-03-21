
using HMS_Backend.Domain.Enums;

namespace HMS_Backend.Application.DTOs.Appointment;

public class UpdateAppointmentStatusDto
{
    public AppointmentStatus Status { get; set; }
    public string? CancellationReason { get; set; }
}