using FluentValidation;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(validator =>
                validator.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Select(error =>
                Error.Validation(
                    error.ErrorCode,
                    error.ErrorMessage))
            .ToList();

        if (errors.Count == 0)
        {
            return await next(cancellationToken);
        }

        return (dynamic)errors;
    }
}