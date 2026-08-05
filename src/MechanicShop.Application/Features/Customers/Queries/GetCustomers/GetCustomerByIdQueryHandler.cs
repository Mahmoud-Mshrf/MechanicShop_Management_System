using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Common.Models;
using MechanicShop.Application.Features.Customers.Dtos;
using MechanicShop.Application.Features.Customers.Mappers;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomers;

public sealed class GetCustomersQueryHandler(IAppDbContext context, ILogger<GetCustomersQuery> logger) : IRequestHandler<GetCustomersQuery, Result<PaginatedList<CustomerDto>>>
{
    public async Task<Result<PaginatedList<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await context.Customers.Include(x=>x.Vehicles).ToListAsync(cancellationToken);
        logger.LogInformation("customers retrieved");
        return customers.ToDtos().Paginate(request.Page,request.PageSize);
    }
}