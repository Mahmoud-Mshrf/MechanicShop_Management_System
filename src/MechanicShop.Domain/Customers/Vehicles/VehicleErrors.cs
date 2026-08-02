using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Customers.Vehicles;

public static class VehicleErrors
{
    public static Error MakeRequired
        => Error.Validation("Vehicle_Make_Is_Required","Vehicle make is required");

    public static Error ModelRequired
        => Error.Validation("Vehicle_Model_Is_Required","Vehicle model is required");
    
    public static Error InvalidYear
        => Error.Validation("Vehicle_Year_Is_Invalid","Vehicle year is invalid , must be 1886 and more");

    public static Error LicensePlateRequired
        => Error.Validation("Vehicle_LicensePlate_Is_Required","Vehicle license plate is required");
}