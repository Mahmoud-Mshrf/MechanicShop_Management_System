using MechanicShop.Domain.Customers.Vehicles;

namespace MechanicShop.Application.Features.Customers.Commands.Dtos;

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
    public Guid Id {get;set;}
    public string Make {get;set;}
    public string Model {get;set;}
    public string LicensePlate {get;set;}
    public int Year {get;set;}
}