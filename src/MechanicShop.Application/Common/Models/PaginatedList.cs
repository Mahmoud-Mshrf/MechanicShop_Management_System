using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Application.Common.Models;

public class PaginatedList<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }

    public IReadOnlyCollection<T>? Items { get; init; }
}
public static class Pagination 
{
    public static PaginatedList<T> Paginate<T>(this IEnumerable<T> values,int page, int size) where T : class
    {
        var items = values.Skip((page - 1 ) * size).Take(size).ToList().AsReadOnly();
        var count = values.Count();
        return new PaginatedList<T>
        {
            Items = items,
            PageNumber = page,
            PageSize=size,
            TotalPages = count/size,
            TotalCount =count
        };
    }
}