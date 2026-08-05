using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidator():AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x=>x.Guid).NotEmpty().WithMessage("Customer id can't be empty");
    }
}