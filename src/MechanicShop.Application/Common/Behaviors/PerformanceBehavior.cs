using System.Diagnostics;
using System.Security.Principal;
using MechanicShop.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(Stopwatch stopwatch, IIdentityService identity , IUser user,ILogger<TRequest> logger) :
IPipelineBehavior<TRequest, TResponse> where TRequest:notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        stopwatch.Start();
        var response = await next();;
        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        if (elapsedMilliseconds > 500)
        {
            var requestName= typeof(TRequest).Name;
            var userId = user.Id?? string.Empty;
            var userName = string.Empty; 
            if (!string.IsNullOrEmpty(userId))
            {
                userName = await identity.GetUserNameAsync(userId);
            }
            
            logger.LogWarning("Long running task : {requestName} ({milliseconds} milliseconds) {@userId} {@userName} {@request}",requestName,elapsedMilliseconds,userId,userName,request);
        }
        return response;
    }
}