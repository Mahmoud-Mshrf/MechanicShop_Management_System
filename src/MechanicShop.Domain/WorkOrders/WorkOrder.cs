using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Constants;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers.Vehicles;
using MechanicShop.Domain.Employees;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.WorkOrders.Enums;

namespace MechanicShop.Domain.WorkOrders;

public sealed class WorkOrder : AuditableEntity
{
    public DateTimeOffset StartAtUtc {get;private set;}
    public DateTimeOffset EndAtUtc {get;private set;}
    public Spot Spot {get;private set;}
    public OrderState OrderState {get; private set;}
    public Employee? Labor {get; set;}
    public Vehicle? Vehicle {get; set;}
    private readonly List<RepairTask> _repairTasks = [];
    public IEnumerable<RepairTask> RepairTasks => _repairTasks.AsReadOnly();
    public Guid LaborId {get;}
    public Guid VehicleId {get;}
    // public Invoice? Invoice {get;private set;}

    public decimal? Discount {get ; private set;} = 0;
    public decimal? Tax {get; private set;}
    // public decimal? TotalPartsCost => RepairTasks.Sum(x=>x.Parts.Sum(p=>p.Cost * p.Quantity)); or
    public decimal? TotalPartsCost => _repairTasks.SelectMany(rt => rt.Parts).Sum(p => p.Cost * p.Quantity);
    public decimal? TotalLaborCost => RepairTasks.Sum(x=>x.LaborCost);
    public decimal? TotalCost => (TotalLaborCost ?? 0) + (TotalPartsCost?? 0);

    private WorkOrder(Guid id,DateTimeOffset startAtUtc, DateTimeOffset endAtUtc, Spot spot, OrderState orderState, Guid laborId, Guid vehicleId, List<RepairTask> repairTasks):base(id)
    {
        StartAtUtc = startAtUtc;
        EndAtUtc = endAtUtc;
        Spot = spot;
        OrderState = orderState;
        LaborId = laborId;
        VehicleId = vehicleId;
        _repairTasks = repairTasks;
    }
    private WorkOrder()
    {}

    public static Result<WorkOrder> Create(Guid id,DateTimeOffset startAtUtc, DateTimeOffset endAtUtc, Spot spot,Guid laborId, Guid vehicleId, List<RepairTask> repairTasks)
    {
        if (id == Guid.Empty)
        {
            return WorkOrderErrors.IdIsRequired;
        }
        if (laborId == Guid.Empty)
        {
            return WorkOrderErrors.LaborIdIsRequired;
        }
        if (vehicleId == Guid.Empty)
        {
            return WorkOrderErrors.VehicleIdIsRequired;
        }
        if (repairTasks is null || repairTasks.Count < 1)
        {
            return WorkOrderErrors.AtLeastOneTaskRequired;
        }
        if (startAtUtc <= endAtUtc)
        {
            return WorkOrderErrors.EndAtMustBeAfterStartAt;
        }
        if (!Enum.IsDefined(spot))
        {
            return WorkOrderErrors.InvalidSpot;
        }

        return new WorkOrder(id,startAtUtc,endAtUtc,spot,OrderState.Scheduled,laborId,vehicleId,repairTasks);
    }
    public bool IsEditable => OrderState == OrderState.Scheduled;

    public Result<Updated> AddRepairTask(RepairTask repairTask)
    {
        if (!IsEditable)
        {
            return WorkOrderErrors.Readonly;
        }
        if (_repairTasks.Any(r=>r.Id==repairTask.Id))
        {
            return WorkOrderErrors.RepairTaskAlreadyIncluded;            
        }
        _repairTasks.Add(repairTask);

        return Result.Updated;
    }

    public Result<Updated> UpdateTiming(DateTimeOffset startAtUtc ,DateTimeOffset endAtUtc)
    {
        if (!IsEditable)
        {
            return WorkOrderErrors.Readonly;
        }
        if (startAtUtc < endAtUtc)
        {
            return WorkOrderErrors.EndAtMustBeAfterStartAt;
        }
        if (StartAtUtc <= DateTime.UtcNow)
        {
            return WorkOrderErrors.StartAtMustBeInTheFuture;
        }
        StartAtUtc = startAtUtc;
        EndAtUtc = endAtUtc;

        return Result.Updated;
    }
}
public static class WorkOrderErrors
{
    public static Error IdIsRequired
        => Error.Validation("Id_Is_Required","WorkOrder id is required ");
    public static Error VehicleIdIsRequired
        => Error.Validation("Vehicle_Id_Is_Required","Vehicle id is required ");
    public static Error LaborIdIsRequired
        => Error.Validation("Labor_Id_Is_Required","Labor id is required ");
    public static Error AtLeastOneTaskRequired
        => Error.Validation("AtLeast_One_Task_Is_Required","At least one repair task is required");

    public static Error EndAtMustBeAfterStartAt
        => Error.Validation("EndAt_MustBe_After_StartAt","Ending time must be after starting time");
    
    public static Error StartAtMustBeInTheFuture
        => Error.Validation("StartAt_MustBe_In_TheFuture","StartAt must be in the future");

    public static Error RepairTaskAlreadyIncluded
        => Error.Validation("RepairTask_Already_Included","RepairTask already included in the current repairTasks");

    public static Error InvalidSpot
        => Error.Validation("Invalid_Spot","Spot is invalid must be A , B , C Or D");

    public static Error Readonly => Error.Conflict(
    code: "WorkOrderErrors.Readonly",
    description: "WorkOrder is read-only or is not editable.");
}


