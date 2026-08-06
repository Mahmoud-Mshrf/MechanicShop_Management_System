using FluentValidation;

namespace MechanicShop.Application.Features.Parts.Commands.CreatePart;

public class CreatePartCommandValidator : AbstractValidator<CreatePartCommand>
{
    public CreatePartCommandValidator()
    {
        RuleFor(x=>x.Name)
        .NotEmpty().WithMessage("Name is required");

        RuleFor(x=>x.Cost)
        .GreaterThan(0).WithMessage("Part cost must be greater than 0");
    }
}