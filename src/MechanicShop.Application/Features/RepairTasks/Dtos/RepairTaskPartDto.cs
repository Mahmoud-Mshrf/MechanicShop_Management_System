namespace MechanicShop.Application.Features.RepairTasks.Dtos;

public class RepairTaskPartDto
{
    public Guid Id {get;set;}
    public int Quantity{get;set;}
}

public class ReturnRepairTaskPartDto:RepairTaskPartDto
{
    public string Name {get;set;}
    public decimal Cost {get;set;}
    public decimal TotalCost {get;set;}
}