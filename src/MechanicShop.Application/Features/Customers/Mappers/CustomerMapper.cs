using System.Linq.Expressions;
using MechanicShop.Application.Features.Customers.Dtos;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;

namespace MechanicShop.Application.Features.Customers.Mappers;

public static class CustomerMapper
{
    public static readonly Expression<Func<Customer, CustomerDto>> ToDtoQueryable =
        customer => new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name!,
            Email = customer.Email!,
            PhoneNumber = customer.PhoneNumber!,
            Vehicles = customer.Vehicles
                .Select(v => new VehicleDto(
                    v.Id,
                    v.Make!,
                    v.Model!,
                    v.LicensePlate!,
                    v.Year))
                .ToList()
        };
    public static CustomerDto ToDto(this Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerDto
        {
            Id = entity.Id,
            Name = entity.Name!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!,
            Vehicles = entity.Vehicles?.Select(v => v.ToDto()).ToList() ?? []
        };
    }

    public static List<CustomerDto> ToDtos(this IEnumerable<Customer> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }

    public static VehicleDto ToDto(this Vehicle entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new VehicleDto(entity.Id, entity.Make!, entity.Model!, entity.LicensePlate! ,entity.Year);
    }

    public static List<VehicleDto> ToDtos(this IEnumerable<Vehicle> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}