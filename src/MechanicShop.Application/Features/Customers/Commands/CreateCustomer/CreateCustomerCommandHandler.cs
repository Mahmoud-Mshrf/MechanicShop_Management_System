using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Customers.Dtos;
using MechanicShop.Application.Features.Customers.Mappers;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(IAppDbContext context,ILogger<CreateCustomerCommandHandler> logger,HybridCache cache) : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(x=>x.Email!.ToLower().Trim()==request.Email.ToLower().Trim());
        if (customer is not null)
        {
            logger.LogWarning("Customer creation aborted , Email already exists");
            return CustomerError.ExistedEmail;
        }

        List<Vehicle> vehicles =[];

        foreach (var v in request.vehicles)
        {
            var creationResult = Vehicle.Create(Guid.NewGuid(),v.Make,v.Model,v.Year,v.LicensePlate);

            if (creationResult.IsError)
            {
                logger.LogWarning("Vehicle creation aborted");
                return creationResult.Errors;
            }
            vehicles.Add(creationResult.Value);
        }

        var createdCustomer = Customer.Create(Guid.NewGuid(),request.Name,request.PhoneNumber,request.Email,vehicles);

        if (createdCustomer.IsError)
        {
            return createdCustomer.Errors;
        }

        await context.Customers.AddAsync(createdCustomer.Value, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Customer with email : {email} was created successfully",request.Email);
        await cache.RemoveByTagAsync("Customers");
        
        return createdCustomer.Value.ToDto();

    }
}
