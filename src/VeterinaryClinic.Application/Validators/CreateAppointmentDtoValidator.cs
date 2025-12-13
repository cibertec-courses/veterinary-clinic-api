using System.Data;
using FluentValidation;
using VeterinaryClinic.Application.DTOs.Appointment;

namespace VeterinaryClinic.Application.Validators
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(a => a.PetId)
                .GreaterThan(0).WithMessage("Pet ID must be greater than 0");

            RuleFor(a => a.AppointmentDate)
                .NotEmpty().WithMessage("Appointment date is required")
                .GreaterThan(DateTime.Now).WithMessage("Appointment date must be in the future");

            RuleFor(a => a.Reason)
                .NotEmpty().WithMessage("Reason for appointment is required")
                .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters");            

            RuleFor(a => a.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");   
        }
    }
}