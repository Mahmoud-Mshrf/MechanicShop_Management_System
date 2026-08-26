using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

public class CachingBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICachedQuery cachedRequest)
        {
            return await next(cancellationToken);
        }

        logger.LogInformation(
            "Checking cache for {RequestName}",
            typeof(TRequest).Name);

        try
        {
            return await cache.GetOrCreateAsync(
                cachedRequest.CacheKey,
                async ct =>
                {
                    var innerResult = await next(ct);

                    if (innerResult is IResult { IsSuccess: true })
                    {
                        return innerResult;
                    }

                    // Prevent HybridCache from persisting a failure/null result.
                    throw new UncacheableResultException<TResponse>(innerResult);
                },
                new HybridCacheEntryOptions
                {
                    Expiration = cachedRequest.Expiration
                },
                cachedRequest.Tags,
                cancellationToken);
        }
        catch (UncacheableResultException<TResponse> ex)
        {
            return ex.Result;
        }
    }
}

internal sealed class UncacheableResultException<TResponse>(TResponse result) : Exception
{
    public TResponse Result { get; } = result;
}