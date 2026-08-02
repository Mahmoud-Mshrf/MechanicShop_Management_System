using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MechanicShop.Domain.RepairTasks.Parts;

namespace MechanicShop.Domain.RepairTasks;

public sealed class RepairTask:AuditableEntity
{
    public string? Name {get;private set;}
    public decimal LaborCost {get; private set;}
    public RepairDurationInMinutes EstimatedDurationInMinutes {get;private set;}
    private readonly List<Part>? _parts = [];
    public IEnumerable<Part> Parts => _parts!.AsReadOnly();
    public  decimal TotalCost => Parts!.Sum(p=>p.Cost * p.Quantity);

    private RepairTask(Guid id,string? name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMinutes, List<Part>? parts):base(id)
    {
        Name = name;
        LaborCost = laborCost;
        EstimatedDurationInMinutes = estimatedDurationInMinutes;
        _parts = parts;
    }
    public static Result<RepairTask> Create(Guid id, string name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMins, List<Part> parts)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RepairTaskErrors.NameRequired;
        }

        if (laborCost <= 0)
        {
            return RepairTaskErrors.LaborCostInvalid;
        }

        if (!Enum.IsDefined(estimatedDurationInMins))
        {
            return RepairTaskErrors.DurationInvalid;
        }

        return new RepairTask(id, name.Trim(), laborCost, estimatedDurationInMins, parts);
    }
    public Result<Updated> Upsert(List<Part> incomingParts)
    {
        if (incomingParts.Count <=0 || incomingParts is null)
        {
            return RepairTaskErrors.PartsRequired;
        }
        _parts!.RemoveAll(existingPart => incomingParts.All(incoming => incoming.Id != existingPart.Id));

        foreach (var part in incomingParts)
        {
            var existing = _parts.FirstOrDefault(x=>x.Id == part.Id);
            if (existing is null)
            {
                _parts.Add(existing!);
            }
            else
            {
                existing.Update(part.Name!,part.Cost,part.Quantity);
            }
        }
        return Result.Updated;
    }
    public Result<Updated> Update(string name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMins)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return RepairTaskErrors.NameRequired;
        }

        if (laborCost <= 0 || laborCost > 10000)
        {
            return RepairTaskErrors.LaborCostInvalid;
        }

        if (!Enum.IsDefined(estimatedDurationInMins))
        {
            return RepairTaskErrors.DurationInvalid;
        }

        Name = name.Trim();
        LaborCost = laborCost;
        EstimatedDurationInMinutes = estimatedDurationInMins;

        return Result.Updated;
    }

}
