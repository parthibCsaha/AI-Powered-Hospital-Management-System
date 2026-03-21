

using FluentValidation;
using HMS_Backend.Application.DTOs.Patient;


namespace HMS_Backend.Application.Validators.Patient;



public class UpdatePatientValidator : AbstractValidator<UpdatePatientDto>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{6,14}$");
        RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
    }
}