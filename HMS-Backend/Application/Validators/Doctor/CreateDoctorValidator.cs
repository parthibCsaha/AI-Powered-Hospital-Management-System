
using FluentValidation;
using HMS_Backend.Application.DTOs.Doctor;


namespace HMS_Backend.Application.Validators.Doctor;


public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{6,14}$");
        RuleFor(x => x.Specialization).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LicenseNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).LessThan(70);
        RuleFor(x => x.ConsultationFee).GreaterThan(0).WithMessage("Consultation fee must be greater than 0.");
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Department is required.");
        RuleFor(x => x.Gender).IsInEnum();
    }
}