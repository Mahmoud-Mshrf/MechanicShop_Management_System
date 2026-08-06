using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.Parts;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public class RepairTaskPart
{
    public Guid PartId {get;private set;}
    public Guid RepairTaskId {get; private set;}
    public RepairTask RepairTask {get;private set;}
    public Part Part {get;private set;}
    public int Quantity{get; private set;}

    private RepairTaskPart()
    {
        
    }

    private RepairTaskPart(Guid partId,int quantity)
    {
        PartId = partId;
        Quantity =quantity;
    }

    public static Result<RepairTaskPart> Create(Guid partId,int quantity)
    {
        return new RepairTaskPart(partId,quantity);
    }
}