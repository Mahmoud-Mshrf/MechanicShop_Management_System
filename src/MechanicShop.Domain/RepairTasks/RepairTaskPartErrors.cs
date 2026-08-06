using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.RepairTasks;

public static class RepairTaskPartErrors
{
    public static Error InvalidPartId
        => Error.Validation("Invalid_Part_Id","Invalid part id");

    public static Error InvalidQuantity
        => Error.Validation("Invalid_Quantity","Invalid part quantity");
}