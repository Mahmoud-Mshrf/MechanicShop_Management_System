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
    public IEnumerable<RepairTask> RepairTasks {get;private set;} = [];
    public Guid LaborId {get;}
    public Guid VehicleId {get;}
    // public Invoice? Invoice {get;private set;}

    public decimal Discount {get ; private set;} = 0;
    public decimal Tax {get; private set;} = MechanicShopConstants.Tax;
    public decimal TotalPartsCost => RepairTasks.Sum(x=>x.Parts.Sum(p=>p.Cost));
    public decimal TotalLaborCost => RepairTasks.Sum(x=>x.LaborCost);
    public decimal TotalCost => TotalLaborCost + TotalPartsCost;

    private WorkOrder(Guid id,DateTimeOffset startAtUtc, DateTimeOffset endAtUtc, Spot spot, OrderState orderState, Employee? labor, Vehicle? vehicle, IEnumerable<RepairTask> repairTasks):base(id)
    {
        StartAtUtc = startAtUtc;
        EndAtUtc = endAtUtc;
        Spot = spot;
        OrderState = orderState;
        Labor = labor;
        Vehicle = vehicle;
        RepairTasks = repairTasks;
    }
    private WorkOrder()
    {}

    public static Result<WorkOrder> Create(Guid id,DateTimeOffset startAtUtc, DateTimeOffset endAtUtc, Spot spot,Employee? labor, Vehicle? vehicle, IEnumerable<RepairTask> repairTasks)
    {
        if (id == Guid.Empty)
        {
            return WorkOrderErrors.IdIsRequired;
        }
        if (repairTasks.Count() <1)
        {
            return WorkOrderErrors.AtLeastOneTaskRequired;
        }
        if (startAtUtc < endAtUtc)
        {
            return WorkOrderErrors.EndAtMustBeAfterStartAt;
        }
        if (!Enum.IsDefined(spot))
        {
            return WorkOrderErrors.InvalidSpot;
        }

        return new WorkOrder(id,startAtUtc,endAtUtc,spot,OrderState.Scheduled,labor,vehicle,repairTasks);
    }
}
public static class WorkOrderErrors
{
    public static Error IdIsRequired
        => Error.Validation("Id_Is_Required","WorkOrder id is required ");
    
    public static Error AtLeastOneTaskRequired
        => Error.Validation("AtLeast_One_Task_Is_Required","At least one repair task is required");

    public static Error EndAtMustBeAfterStartAt
        => Error.Validation("EndAt_MustBe_After_StartAt","Ending time must be after starting time");

    public static Error InvalidSpot
        => Error.Validation("Invalid_Spot","Spot is invalid must be A , B , C Or D");
}


