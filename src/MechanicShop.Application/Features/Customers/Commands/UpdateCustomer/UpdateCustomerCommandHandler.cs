using System.Data.Common;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(IAppDbContext context,ILogger<UpdateCustomerCommandHandler> logger, HybridCache cache) : IRequestHandler<UpdateCustomerCommand, Result<Updated>>{
    public async Task<Result<Updated>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.Include(x=>x.Vehicles).FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (customer is null)
        {
            return CustomerError.NotFound(request.Id.ToString());
        }

        var updateResult =  customer.Update(request.Name,request.PhoneNumber,request.Email);
        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }
        List<Vehicle> vehicles = [];
        foreach (var v in request.vehicles)
        {
            var id = v.Id ?? Guid.NewGuid();
            var vehicle = Vehicle.Create(id,v.Make,v.Model,v.Year,v.LicensePlate);
            if (vehicle.IsError)
            {
                return vehicle.Errors;
            }
            vehicles.Add(vehicle.Value);
        }

        var updatePartsResult = customer.Upsert(vehicles);

        if (updatePartsResult.IsError)
        {
            return updatePartsResult.Errors;
        }
        await context.SaveChangesAsync(cancellationToken);

        await cache.RemoveByTagAsync("Customers", cancellationToken);

        return Result.Updated;
    }
}
