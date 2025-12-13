
using FluentValidation;
using VeterinaryClinic.Application.DTOs.Pet;

namespace VeterinaryClinic.Application.Validators
{
    public class CreatePetDtoValidator : AbstractValidator<CreatePetDto>
    {

        public CreatePetDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithName("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(p => p.Species)
                .NotEmpty().WithMessage("Species is required")
                .MaximumLength(50).WithMessage("Species cannot exceed 50 characters");

            RuleFor(p => p.Breed)
                .MaximumLength(50).WithMessage("Breed cannot exceed 50 characters");

            RuleFor(x=> x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Birth date cannot be in the future");

            RuleFor(p => p.OwnerId)
                .GreaterThan(0).WithMessage("Owner ID must be greather than 0");

        }


    }
}