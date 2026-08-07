using FluentValidation;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest> validator)
: IPipelineBehavior<TRequest, TResponse>
where TRequest :IRequest<TResponse> 
where TResponse:IResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next(cancellationToken);
        }

        var result = await validator.ValidateAsync(request,cancellationToken);

        if (result.IsValid)
        {
            return await next(cancellationToken);
        }

        var errors = result.Errors.ConvertAll(error => Error.Validation(error.ErrorCode,error.ErrorMessage));

        return (dynamic) errors;
    }
}
