using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.RepairTasks.Parts
{
    public class Part:AuditableEntity
    {
        public string? Name {get;private set;}
        public decimal Cost{get;private set;}
        public int Quantity{get;private set;}

        private Part()
        {
            
        }

        private Part(Guid id,string name , decimal cost,int quantity):base(id)
        {
            Name = name;
            Cost=cost;
            Quantity = quantity;
        }

        public static Result<Part> Create(Guid id,string name , decimal cost,int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PartErrors.NameRequired;
            }

            if (0 >= cost || cost > 10000)
            {
                return PartErrors.InvalidCost;
            }
            if (0>= quantity || quantity > 10)
            {
                return PartErrors.InvalidQuantity;
            }

            return new Part(id,name,cost,quantity);
        }

        public  Result<Updated> Update(string name , decimal cost,int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PartErrors.NameRequired;
            }

            if (0 >= cost || cost > 10000)
            {
                return PartErrors.InvalidCost;
            }
            if (0>= quantity || quantity > 10)
            {
                return PartErrors.InvalidQuantity;
            }

            Name =name;
            Cost= cost;
            Quantity=quantity;

            return Result.Updated;
        }
    }
}