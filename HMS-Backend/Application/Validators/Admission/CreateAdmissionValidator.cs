
using FluentValidation;
using HMS_Backend.Application.DTOs.Admission;

namespace HMS_Backend.Application.Validators.Admission;


public class CreateAdmissionValidator : AbstractValidator<CreateAdmissionDto>
{
    public CreateAdmissionValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.DoctorId).NotEmpty();
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.AdmissionDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddHours(1));
        RuleFor(x => x.AdmissionReason).NotEmpty().MaximumLength(500);
    }
}