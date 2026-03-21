
using FluentValidation; 
using HMS_Backend.Application.DTOs.Appointment;


namespace HMS_Backend.Application.Validators.Appointment;


public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Appointment date cannot be in the past.");
        RuleFor(x => x.StartTime)
            .Must(t => t.Hours >= 6 && t.Hours < 22)
            .WithMessage("Appointment must be between 06:00 and 22:00.");
    }
}