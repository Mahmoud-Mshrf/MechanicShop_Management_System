using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.WorkOrders.Enums;

namespace MechanicShop.Domain.WorkOrders;

public static class WorkOrderErrors
{
    public static Error IdIsRequired
        => Error.Validation("Id_Is_Required","WorkOrder id is required ");
    public static Error VehicleIdIsRequired
        => Error.Validation("Vehicle_Id_Is_Required","Vehicle id is required ");
    public static Error LaborIdIsEmpty(string guid)
        => Error.Validation("Labor_Id_Is_Empty",$"Labor id {guid} is empty ");
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

    public static Error Readonly 
        => Error.Conflict("WorkOrderErrors.Readonly","WorkOrder is read-only or is not editable .");

    public static Error InvalidStateTransition(OrderState olderState,OrderState newState) 
        => Error.Conflict("Invalid_State_Transition",$"Invalid state transition from {olderState} to {newState} .");
}


