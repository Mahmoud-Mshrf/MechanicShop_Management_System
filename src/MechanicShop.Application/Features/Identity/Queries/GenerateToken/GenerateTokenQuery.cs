using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Identity.Queries.GenerateToken;

public sealed record GenerateTokenQuery(string Email,string Password):IRequest<Result<TokenResponse>>;

public class GenerateTokenQueryHandler(ITokenProvider tokenProvider) : IRequestHandler<GenerateTokenQuery, Result<TokenResponse>>
{
    public Task<Result<TokenResponse>> Handle(GenerateTokenQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}