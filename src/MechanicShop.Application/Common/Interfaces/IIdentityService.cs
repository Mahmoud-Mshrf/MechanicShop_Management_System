using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<Result<AppUserDto>> GetUserByIdAsync(string userId);
    Task<Result<AppUserDto>> AuthenticateAsync(string Email,string Password);
    Task<string?> GetUserNameAsync(string userId);
}
