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

public sealed class GetCustomersQueryHandler(
    IAppDbContext context,
    ILogger<GetCustomersQuery> logger)
    : IRequestHandler<GetCustomersQuery, Result<PaginatedList<CustomerDto>>>
{
    public async Task<Result<PaginatedList<CustomerDto>>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await context.Customers.Include(x=>x.Vehicles)
            .AsNoTracking()
            .Select(CustomerMapper.ToDtoQueryable)
            .PaginateAsync(request.Page, request.PageSize, cancellationToken);

        logger.LogInformation("Customers retrieved.");

        return customers;
    }
}