using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Customer.Vehicle;

public class Vehicle : AuditableEntity
{
    public Guid CustomerId {get;}
    public string? Make {get; private set;}
    public string? Model {get; private set;}
    public string? LicensePlate {get; private set;}
    public int Year {get; private set;}
    public Customer? Customer {get;set;}

    public string VehicleInfo => $"{Make} | {Model} | {Year}";

    private Vehicle()
    {}

    private Vehicle(Guid id, string make , string model , int year,string licensePlate):base(id)
    {
        Make =make;
        Model=model;
        Year = year;
        LicensePlate = licensePlate;
    }

    public static Result<Vehicle> Create(Guid id, string make , string model , int year,string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(make))
        {
            return VehicleErrors.MakeRequired;
        }
        if (string.IsNullOrWhiteSpace(model))
        {
            return VehicleErrors.ModelRequired;
        }

        if (string.IsNullOrWhiteSpace(licensePlate))
        {
            return VehicleErrors.LicensePlateRequired;
        }
        if (year < 1886 || year > DateTime.UtcNow.Year )
        {
            return VehicleErrors.InvalidYear;
        }

        return new Vehicle(id,make,model,year,licensePlate);
    }
}
public static class VehicleErrors
{
    public static Error MakeRequired
        => Error.Validation("Vehicle_Make_Is_Required","Vehicle make is required");

    public static Error ModelRequired
        => Error.Validation("Vehicle_Model_Is_Required","Vehicle model is required");
    
    public static Error InvalidYear
        => Error.Validation("Vehicle_Year_Is_Invalid","Vehicle year is invalid");

    public static Error LicensePlateRequired
        => Error.Validation("Vehicle_LicensePlate_Is_Required","Vehicle license plate is required");

    
}