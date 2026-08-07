using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MediatR;

namespace MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public sealed record UpdateRepairTaskCommand(Guid Id,string Name,decimal LaborCost,RepairDurationInMinutes EstimatedDurationInMinutes,List<RepairTaskPartDto> RepairTaskPartDtos):IRequest<Result<Updated>>;
