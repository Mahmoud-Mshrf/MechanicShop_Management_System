using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Identity.Queries.GenerateToken;

public class GenerateTokenQueryHandler(ITokenProvider tokenProvider,IIdentityService identityService,ILogger<GenerateTokenQueryHandler> logger) : IRequestHandler<GenerateTokenQuery, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(GenerateTokenQuery request, CancellationToken cancellationToken)
    {
        var userDto = await identityService.AuthenticateAsync(request.Email,request.Password);
        if (userDto.IsError)
        {
            return userDto.Errors;
        }
        var token = await tokenProvider.GenerateJwtTokenAsync(userDto.Value);
        if (token.IsError)
        {
            logger.LogError("Generate token error occurred: {ErrorDescription}", token.TopError.Description);
            return token.Errors;
        }
        return token;
    }
}