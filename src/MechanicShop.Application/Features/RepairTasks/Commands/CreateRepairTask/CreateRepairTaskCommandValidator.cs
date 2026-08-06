using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Validators;
using MechanicShop.Application.Features.RepairTasks.Dtos;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public class CreateRepairTaskCommandValidator : AbstractValidator<CreateRepairTaskCommand>
{
    public CreateRepairTaskCommandValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LaborCost)
            .GreaterThan(0).WithMessage("Labor cost must be greater than 0.");

        RuleFor(x => x.EstimatedDurationInMinutes)
            .NotNull().WithMessage("Estimated duration is required.")
            .IsInEnum();

        RuleForEach(x=>x.Dtos)
        .SetValidator(new RepairTaskPartDtoValidator());
        

    }
}

public class RepairTaskPartDtoValidator : AbstractValidator<RepairTaskPartDto>
{
    public RepairTaskPartDtoValidator()
    {
        RuleFor(x=>x.Id)
        .NotEmpty().WithMessage("Part id is required");

        RuleFor(x=>x.Quantity)
        .GreaterThanOrEqualTo(1).WithMessage("Quantity of any part must be equal or greater than 1");
    }
}