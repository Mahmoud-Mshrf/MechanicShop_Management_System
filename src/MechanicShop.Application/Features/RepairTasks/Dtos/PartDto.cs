using MechanicShop.Domain.RepairTasks.Enums;
using Microsoft.VisualBasic;

namespace MechanicShop.Application.Features.RepairTasks.Dtos;
public class PartDto
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public decimal Cost {get;set;}
}

public class RepairTaskDto
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public decimal LaborCost {get;set;}
    public RepairDurationInMinutes  RepairDurationInMinutes {get;set;}
    public IEnumerable<RepairTaskPartDto> Parts{get;set;}
    public decimal TotalCost {get;set;}
}