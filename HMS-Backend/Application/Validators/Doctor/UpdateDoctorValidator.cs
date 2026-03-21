

using FluentValidation;
using HMS_Backend.Application.DTOs.Doctor;

namespace HMS_Backend.Application.Validators.Doctor;


public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{6,14}$");
        RuleFor(x => x.Specialization).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ConsultationFee).GreaterThan(0);
        RuleFor(x => x.DepartmentId).NotEmpty();
    }
}