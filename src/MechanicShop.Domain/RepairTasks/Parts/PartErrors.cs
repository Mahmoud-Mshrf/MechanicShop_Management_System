using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.RepairTasks.Parts
{
    public static class PartErrors
    {
        public static Error NameRequired
            => Error.Validation("PartName_Is_Required","Part name is required");

        public static Error InvalidCost
            => Error.Validation("Part_Invalid_Cost","Part cost is invalid, must be between 1 and 10000");
        
        public static Error InvalidQuantity
            => Error.Validation("Part_Invalid_Quantity","Part Quantity is invalid , must be between 1 and 10");
    }
}