using System.Security.Claims;
using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Identity.Queries.RefreshToken;

public sealed record RefreshTokenQuery(string RefreshToken,string ExpiredAccessToken):IRequest<Result<TokenResponse>>;

public class RefreshTokenQueryHandler(IIdentityService identityService, ITokenProvider tokenProvider, Logger<RefreshTokenQueryHandler> logger,IAppDbContext context) : IRequestHandler<RefreshTokenQuery, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var principal = tokenProvider.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);

        if (principal is null)
        {
            logger.LogError("Expired access token is not valid");

            return ApplicationErrors.ExpiredAccessTokenInvalid;
        }
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            logger.LogError("Invalid userId claim");

            return ApplicationErrors.UserIdClaimInvalid;
        }

        var getUserResult = await identityService.GetUserByIdAsync(userId);

        if (getUserResult.IsError)
        {
            logger.LogError("Get user by id error occurred: {ErrorDescription}", getUserResult.TopError.Description);
            return getUserResult.Errors;
        }

        var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == request.RefreshToken && r.UserId == userId, cancellationToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            logger.LogError("Refresh token has expired");

            return ApplicationErrors.RefreshTokenExpired;
        }

        var generateTokenResult = await tokenProvider.GenerateJwtTokenAsync(getUserResult.Value, cancellationToken);

        if (generateTokenResult.IsError)
        {
            logger.LogError("Generate token error occurred: {ErrorDescription}", generateTokenResult.TopError.Description);

            return generateTokenResult.Errors;
        }

            return generateTokenResult.Value;
        }
}