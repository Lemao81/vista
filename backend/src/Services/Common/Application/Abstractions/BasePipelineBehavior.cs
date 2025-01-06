using System.Reflection;
using Domain;
using Domain.ValueObjects;
using MediatR;

namespace Application.Abstractions;

public abstract class BasePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

	protected TResponse CreateErrorResult(Error error)
	{
		var responseType = typeof(TResponse);
		if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
		{
			var genericResult = Activator.CreateInstance(responseType, BindingFlags.Instance | BindingFlags.NonPublic, null, [error], null);
			ArgumentNullException.ThrowIfNull(genericResult);

			return (TResponse)genericResult;
		}


		if (responseType != typeof(Result))
		{
			throw new ApplicationException("Response type must be Result<> or Result");
		}

		var result = Activator.CreateInstance(responseType, BindingFlags.Instance | BindingFlags.NonPublic, null, [false, error], null);
		ArgumentNullException.ThrowIfNull(result);

		return (TResponse)result;
	}
}
