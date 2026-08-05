using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Commands.DeleteCustomer;

public sealed class DeleteCustomerCommandHandler(IAppDbContext context,ILogger<DeleteCustomerCommandHandler> logger, HybridCache cache) : IRequestHandler<DeleteCustomerCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.FindAsync(request.Guid);
        if (customer is null)
        {
            logger.LogWarning("customer with id {id} not found",request.Guid);
            return ApplicationErrors.CustomerNotFound;
        }

        var hasActiveWorkOrders = context.WorkOrders.Include(x=>x.Vehicle).Where(x=>x.Vehicle!=null).Any(x=>x.Vehicle!.CustomerId== request.Guid);

        if (hasActiveWorkOrders)
        {
            logger.LogWarning("customer with id {id} cant be deleted because he has active work orders",request.Guid);
            return ApplicationErrors.CustomerHasActiveWorkOrders;
        }
        
        context.Customers.Remove(customer);

        await cache.RemoveByTagAsync("Customers");

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("customer with id {id} was deleted successfully",request.Guid);

        return Result.Deleted;
    }
}