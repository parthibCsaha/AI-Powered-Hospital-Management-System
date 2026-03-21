

using FluentValidation;
using HMS_Backend.Application.DTOs.Appointment;


namespace HMS_Backend.Application.Validators.Appointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient is required.");
        RuleFor(x => x.DoctorId).NotEmpty().WithMessage("Doctor is required.");
        RuleFor(x => x.AppointmentDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Appointment date cannot be in the past.");
        RuleFor(x => x.StartTime)
            .NotEmpty()
            .Must(t => t.Hours >= 6 && t.Hours < 22)
            .WithMessage("Appointment must be between 06:00 and 22:00.");
    }
}