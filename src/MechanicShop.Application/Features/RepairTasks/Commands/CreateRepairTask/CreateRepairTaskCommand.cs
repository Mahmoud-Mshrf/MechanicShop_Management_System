using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MediatR;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record CreateRepairTaskCommand(string Name, decimal LaborCost, RepairDurationInMinutes EstimatedDurationInMinutes, List<RepairTaskPartDto> Dtos):IRequest<Result<RepairTaskDto>>;
