using Application.Abstractions;
using FluentValidation;
using MediatR;
using SharedKernel;

namespace Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var results  = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));
		var failures = results.Where(r => !r.IsValid).SelectMany(r => r.Errors).ToArray();
		if (failures.Length == 0)
		{
			return await next();
		}

		var errors = failures.GroupBy(f => f.PropertyName).ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());

		return CreateErrorResult(new ValidationError(ErrorCodes.ValidationFailed, errors));
	}
}
