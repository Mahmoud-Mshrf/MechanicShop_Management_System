using MechanicShop.Application.Features.Customers.Dtos;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;

namespace MechanicShop.Application.Features.Customers.Mappers;

public static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            Email = customer.Email,
            Id = customer.Id,
            Name=customer.Name,
            PhoneNumber=customer.PhoneNumber,
            Vehicles = [.. customer.Vehicles.Select(x=>x.ToDto())]
        };
    }
}
public static class VehicleMapper
{
    public static VehicleDto ToDto(this Vehicle vehicle)
    {
        return new VehicleDto
        {
            Id = vehicle.Id,
            LicensePlate= vehicle.LicensePlate,
            Make= vehicle.Make,
            Model=vehicle.Model,
            Year = vehicle.Year
        };
    }
}