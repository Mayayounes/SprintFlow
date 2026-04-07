using FluentValidation;
using MediatR;

namespace sprintFlow.Application.Common;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var errors = validationResults
            .SelectMany(r => r.Errors)
            .Where(e => e != null)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (errors.Any())
        {
            var resultType = typeof(TResponse);

            if (!resultType.IsGenericType || resultType.GetGenericTypeDefinition() != typeof(Result<>))
                throw new InvalidOperationException("TResponse must be Result<T>");

            var dataType = resultType.GetGenericArguments()[0];

            var failureMethod = typeof(Result<>)
                .MakeGenericType(dataType)
                .GetMethod("Failure", new[] { typeof(List<string>), typeof(string) });

            var failureResult = failureMethod!.Invoke(null, new object[] { errors, "Validation failed" });

            return (TResponse)failureResult!;
        }

        return await next();
    }
}