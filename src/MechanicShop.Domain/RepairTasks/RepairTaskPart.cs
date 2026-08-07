using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Parts;

namespace MechanicShop.Domain.RepairTasks;

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

    public static Result<RepairTaskPart> Create(Guid partId, int quantity)
    {
        if (partId == Guid.Empty)
        {
            return RepairTaskPartErrors.InvalidPartId; // or RepairTaskPartErrors.InvalidPartId
        }

        if (quantity <= 0 || quantity > 10)
        {
            return RepairTaskPartErrors.InvalidQuantity;
        }

        return new RepairTaskPart(partId, quantity);
    }
    public Result<Updated> UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            return RepairTaskPartErrors.InvalidQuantity;

        Quantity = quantity;

        return Result.Updated;
    }
}
