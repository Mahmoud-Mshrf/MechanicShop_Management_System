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
    private readonly List<RepairTaskPart>? _parts = [];
    public IEnumerable<RepairTaskPart> Parts => _parts!.AsReadOnly();
    public  decimal TotalCost => LaborCost + Parts!.Sum(p=>p.Part.Cost * p.Quantity);

    private RepairTask(Guid id,string? name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMinutes):base(id)
    {
        Name = name;
        LaborCost = laborCost;
        EstimatedDurationInMinutes = estimatedDurationInMinutes;
    }
    public static Result<RepairTask> Create(Guid id, string name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMins)
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

        return new RepairTask(id, name.Trim(), laborCost, estimatedDurationInMins);
    }
    public Result<Deleted> RemoveParts(List<Guid> partIds)
    {
        if (partIds is null || partIds.Count == 0)
            return RepairTaskErrors.PartsRequired;

        _parts.RemoveAll(x => partIds.Contains(x.PartId));

        return Result.Deleted;
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
    public Result<Updated> UpdatePartQuantity(Guid partId, int quantity)
    {
        var part = _parts.FirstOrDefault(x => x.PartId == partId);

        if (part is null)
            return RepairTaskErrors.PartNotFound;

        var result = part.UpdateQuantity(quantity);

        if (result.IsError)
            return result.Errors;

        return Result.Updated;
    }
    public Result<Success> AddPart(Guid partId, int quantity)
    {
        if (_parts!.Any(x => x.PartId == partId))
        {
            return RepairTaskErrors.PartAlreadyAdded;
        }

        var creationResult = RepairTaskPart.Create(partId, quantity);

        if (creationResult.IsError)
        {
            return creationResult.Errors;
        }

        _parts.Add(creationResult.Value);

        return Result.Success;
    }
}
