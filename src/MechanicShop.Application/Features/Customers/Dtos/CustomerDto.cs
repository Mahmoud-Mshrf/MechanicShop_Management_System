using MechanicShop.Domain.Customers.Vehicles;

namespace MechanicShop.Application.Features.Customers.Dtos;

public class CustomerDto
{
    public Guid Id {get;set;}
    public string Name {get;set;}
    public string PhoneNumber {get;set;}
    public string Email {get;set;}
    public List<VehicleDto> Vehicles {get;set;}
}

public class VehicleDto
{
    public VehicleDto(Guid id, string make, string model, string licensePlate, int year)
    {
        Id = id;
        Make = make;
        Model = model;
        LicensePlate = licensePlate;
        Year = year;
    }

    public Guid Id {get;set;}
    public string Make {get;set;}
    public string Model {get;set;}
    public string LicensePlate {get;set;}
    public int Year {get;set;}
}