using MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;
using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.Parts;

namespace MechanicShop.Application.Features.RepairTasks.Mappers;

public static class RepairTaskMapper
{
    public static RepairTaskDto ToDto(this RepairTask task)
    {
        return new RepairTaskDto
        {
            Id=task.Id,
            LaborCost=task.LaborCost,
            Name=task.Name,
            RepairDurationInMinutes=task.EstimatedDurationInMinutes,
            TotalCost=task.TotalCost,
            Parts=task.Parts.Select(x=>x.ToDto())
        };
    }
    public static ReturnRepairTaskPartDto ToDto(this RepairTaskPart part)
    {
        var totalCost = part.Part.Cost * part.Quantity;
        return new ReturnRepairTaskPartDto
        {
            Cost=part.Part.Cost,
            Id=part.PartId,
            Name=part.Part.Name!,
            Quantity=part.Quantity,
            TotalCost=totalCost
        };
    }
}