using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Application.Common.Models;

public sealed class PaginatedList<T> where T : class
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }

    public IReadOnlyCollection<T> Items { get; init; } = [];
}
public static class Pagination
{
    public static async Task<PaginatedList<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
        where T : class
    {
        page = Math.Max(page, 1);
        pageSize = Math.Max(pageSize, 1);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>
        {
            Items = items.AsReadOnly(),
            PageNumber = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}