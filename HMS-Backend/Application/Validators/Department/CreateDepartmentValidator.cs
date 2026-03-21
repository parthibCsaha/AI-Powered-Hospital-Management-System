
using FluentValidation;
using HMS_Backend.Application.DTOs.Department;

namespace HMS_Backend.Application.Validators.Department;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PhoneExtension).NotEmpty().MaximumLength(20);
    }
    
}