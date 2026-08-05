using FluentValidation;
using FluentValidation.Validators;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator: AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required")
        .MaximumLength(100);

        RuleFor(x => x.Email)
        .EmailAddress().WithMessage("Invalid email")
        .MaximumLength(100);

        RuleFor(x => x.PhoneNumber)
        .NotEmpty().WithMessage("Phone number is required.")
        .Matches(@"^\+?\d{7,15}$").WithMessage("Phone number must be 7–15 digits and may start with '+'.");

        RuleFor(x=>x.vehicles)
        .NotNull().WithMessage("Vehicles list cannot be null")
        .Must(x=>!(x.Count<=0)).WithMessage("A customer must have at least one car");

        RuleForEach(x=>x.vehicles)
        .SetValidator(new UpdateVehicleCommandValidator());
    }
}
