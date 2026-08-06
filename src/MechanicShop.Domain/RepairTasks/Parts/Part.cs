using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.RepairTasks.Parts
{
    public sealed class Part:AuditableEntity
    {
        public string? Name {get;private set;}
        public decimal Cost{get;private set;}

        private Part()
        {
            
        }

        private Part(Guid id,string name , decimal cost):base(id)
        {
            Name = name;
            Cost=cost;
        }

        public static Result<Part> Create(Guid id,string name , decimal cost)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PartErrors.NameRequired;
            }

            if (0 >= cost || cost > 10000)
            {
                return PartErrors.InvalidCost;
            }

            return new Part(id,name,cost);
        }

        public  Result<Updated> Update(string name , decimal cost)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PartErrors.NameRequired;
            }

            if (0 >= cost || cost > 10000)
            {
                return PartErrors.InvalidCost;
            }
            Name =name;
            Cost= cost;

            return Result.Updated;
        }
    }
}