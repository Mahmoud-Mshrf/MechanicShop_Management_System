using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Customers.Dtos;
using MechanicShop.Application.Features.Customers.Mappers;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler(IAppDbContext context, ILogger<GetCustomerByIdQueryHandler> logger) : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer =await context.Customers.AsNoTracking().Include(x=>x.Vehicles).FirstOrDefaultAsync(x=>x.Id==request.Id);
        if (customer is null)
        {
            logger.LogWarning("Customer with id {id} not found",request.Id);
            return ApplicationErrors.CustomerNotFound;
        }

        logger.LogInformation("Customer with id {id} was retrieved",request.Id);
        return customer.ToDto();

    }
}