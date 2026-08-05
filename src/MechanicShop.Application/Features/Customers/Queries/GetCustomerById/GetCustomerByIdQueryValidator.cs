using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Customer id can't be empty");
    }
}