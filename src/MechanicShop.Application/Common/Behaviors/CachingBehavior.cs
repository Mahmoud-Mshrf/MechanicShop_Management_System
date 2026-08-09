using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse>(HybridCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICachedQuery cachedRequest)
        {
            return await next(cancellationToken);
        }

        logger.LogInformation("Checking cache for {requestName}",typeof(TRequest).Name);
        var result =await cache.GetOrCreateAsync(cachedRequest.CacheKey,factory :async cancellationToken =>
        {
            var innerResult = await next(cancellationToken);
            if (innerResult is IResult r && r.IsSuccess)
            {
                return innerResult;
            }

            // in case of failures (don't cache failures):
            return default;
        },options: new HybridCacheEntryOptions
        {
            Expiration = cachedRequest.Expiration
        },tags: cachedRequest.Tags,
        cancellationToken:cancellationToken);

        return result!;
    }
}