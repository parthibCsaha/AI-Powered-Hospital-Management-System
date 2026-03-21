

using FluentValidation;
using HMS_Backend.Application.DTOs.MedicalRecord;

namespace HMS_Backend.Application.Validators.MedicalRecord;



public class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordDto>
{
    public CreateMedicalRecordValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.DoctorId).NotEmpty();
        RuleFor(x => x.Diagnosis).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Treatment).NotEmpty().MaximumLength(2000);
    }
}
 
