using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Application.Common.Errors;
public static class ApplicationErrors
{
    public static Error WorkOrderOutsideOperatingHour(DateTimeOffset startAtUtc, DateTimeOffset endAtUtc) =>
    Error.Conflict(
           "ApplicationErrors.WorkOrder.Outside.OperatingHours",
           $"The WorkOrder time ({startAtUtc} ? {endAtUtc}) is outside of store operating hours.");

    public static Error WorkOrderNotFound => Error.NotFound(
           "ApplicationErrors.WorkOrder.NotFound",
           "WorkOrder does not exist.");

    public static Error LaborOccupied =>
    Error.Conflict(
           "Employee.LaborOccupied",
           "Labor is already occupied during the requested time.");

    public static Error RepairTaskWithSameNameAlreadyExists =>
    Error.Conflict(
           "Repair_Task_With_Same_Name_Already_Exists",
           "Repair_Task_With_Same_Name_Already_Exists.");

    public static Error CustomerNotFound =>
    Error.NotFound(
           "ApplicationErrors.Customer.NotFound",
           "Customer does not exist.");

    public static Error PartNotFound =>
    Error.NotFound(
           "ApplicationErrors.Parts.NotFound",
           "Part does not exist.");

    public static Error VehicleNotFound =>
    Error.NotFound(
           "ApplicationErrors.Vehicle.NotFound",
           "Vehicle does not exist.");

    public static Error VehicleSchedulingConflict =>
    Error.Conflict(
            "Vehicle_Overlapping_WorkOrder",
            "The vehicle already has an overlapping WorkOrder.");

    public static Error PartWithSameNameAlreadyCreated =>
    Error.Conflict(
            "Part_With_Same_Name_Already_Created",
            "Part with the same name already created.");
    public static Error CustomerHasActiveWorkOrders =>
    Error.Conflict(
            "Customer_Has_Active_Work_Orders",
            "Customer has active work orders.");

    public static Error RepairTaskNotFound =>
    Error.NotFound(
            "RepairTask.NotFound",
            "Repair task does not exist.");

    public static Error WorkOrderMustBeCompletedForInvoicing =>
    Error.Conflict(
            "WorkOrder.InvoiceIssuance.InvalidState",
            "WorkOrder must be in 'Completed' state to issue an invoice.");

    public static Error InvoiceNotFound => Error.NotFound(
       "ApplicationErrors.Invoice.NotFound",
       "Invoice does not exist.");

    public static Error InvalidRefreshToken =>
    Error.Validation(
        "RefreshToken.Expiry.Invalid",
        "Expiry must be in the future.");

    public static readonly Error ExpiredAccessTokenInvalid = Error.Conflict(
         code: "Auth.ExpiredAccessToken.Invalid",
         description: "Expired access token is not valid.");

    public static readonly Error UserIdClaimInvalid = Error.Conflict(
        code: "Auth.UserIdClaim.Invalid",
        description: "Invalid userId claim.");

    public static readonly Error RefreshTokenExpired = Error.Conflict(
        code: "Auth.RefreshToken.Expired",
        description: "Refresh token is invalid or has expired.");

    public static readonly Error UserNotFound = Error.NotFound(
        code: "Auth.User.NotFound",
        description: "User not found.");

    public static readonly Error TokenGenerationFailed = Error.Failure(
        code: "Auth.TokenGeneration.Failed",
        description: "Failed to generate new JWT token.");

    public static Error LaborNotFound =>
        Error.NotFound("Employee.LaborNotFound", "Labor does not exist.");
}