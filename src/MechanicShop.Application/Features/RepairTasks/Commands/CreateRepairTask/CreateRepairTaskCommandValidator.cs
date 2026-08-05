using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Validators;

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
        
        RuleFor(x => x.Parts)
            .NotNull().WithMessage("Parts list cannot be null.")
            .Must(p => p.Count > 0).WithMessage("At least one part is required.");

        RuleForEach(x=>x.Parts)
            .SetValidator(new CreatePartCommandValidator());

    }
}
