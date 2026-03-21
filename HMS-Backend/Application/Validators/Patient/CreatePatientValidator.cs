
using FluentValidation;
using HMS_Backend.Application.DTOs.Patient;

namespace HMS_Backend.Application.Validators.Patient;


public class CreatePatientValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);
 
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);
 
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth cannot be in the future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-150)).WithMessage("Invalid date of birth.");
 
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
 
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{6,14}$").WithMessage("Invalid phone number format.");
 
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(250);
 
        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender.");
 
        RuleFor(x => x.BloodGroup)
            .IsInEnum().WithMessage("Invalid blood group.");
    }
}