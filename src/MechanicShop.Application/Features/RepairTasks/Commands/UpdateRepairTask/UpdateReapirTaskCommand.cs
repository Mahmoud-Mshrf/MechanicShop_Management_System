using FluentValidation;
using MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;
using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MediatR;

namespace MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public sealed record UpdateRepairTaskCommand(Guid Id,string Name,decimal LaborCost,RepairDurationInMinutes EstimatedDurationInMinutes,List<RepairTaskPartDto> RepairTaskPartDtos):IRequest<Result<Updated>>;

public sealed class UpdateRepairTaskCommandValidator
    : AbstractValidator<UpdateRepairTaskCommand>
{
    public UpdateRepairTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Repair task id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LaborCost)
            .GreaterThan(0);

        RuleFor(x => x.EstimatedDurationInMinutes)
            .IsInEnum();

        RuleFor(x => x.RepairTaskPartDtos)
            .NotNull();

        RuleFor(x => x.RepairTaskPartDtos)
            .Must(parts => parts.Select(p => p.Id).Distinct().Count() == parts.Count)
            .WithMessage("A part cannot be added more than once.");

        RuleForEach(x => x.RepairTaskPartDtos)
            .SetValidator(new RepairTaskPartDtoValidator());
    }
}