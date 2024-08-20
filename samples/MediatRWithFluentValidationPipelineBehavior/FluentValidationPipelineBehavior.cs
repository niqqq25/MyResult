using FluentValidation;
using MediatR;
using MyResult;

namespace MediatRWithFluentValidationPipelineBehavior;

public sealed class FluentValidationPipelineBehavior<TRequest, TResponse>(IValidator<TRequest> validator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var validationError = validationResult.Errors.First();
        return (dynamic)new Error(validationError.ErrorCode, validationError.ErrorMessage);
    }
}