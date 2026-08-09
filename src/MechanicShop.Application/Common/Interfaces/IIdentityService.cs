using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<Result<AppUserDto>> GetUserByIdAsync(string userId);

    Task<string?> GetUserNameAsync(string userId);
}