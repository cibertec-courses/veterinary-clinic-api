
using FluentValidation;
using VeterinaryClinic.Application.DTOs.Owner;

namespace VeterinaryClinic.Application.Validators
{

    public class CreateOwnerDtoValidator : AbstractValidator<CreateOwnerDto>
    {
        public CreateOwnerDtoValidator()
        {
            RuleFor(o => o.FirstName)
                .NotEmpty().WithMessage("Firstname is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(o => o.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(o => o.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");   
            
            RuleFor(o => o.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("A valid phone number is required");

        }
    }
    
}