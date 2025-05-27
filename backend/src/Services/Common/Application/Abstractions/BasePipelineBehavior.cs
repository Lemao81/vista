using System.Reflection;
using MediatR;
using SharedKernel;

namespace Common.Application.Abstractions;

public abstract class BasePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
{
	public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

	protected static TResponse CreateErrorResult(Error error)
	{
		var responseType = typeof(TResponse);
		if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
		{
#pragma warning disable S3011
			var genericResult = Activator.CreateInstance(responseType, BindingFlags.Instance | BindingFlags.NonPublic, null, [error], null);
#pragma warning restore S3011
			ArgumentNullException.ThrowIfNull(genericResult);

			return (TResponse)genericResult;
		}

		if (responseType != typeof(Result))
		{
			throw new InvalidTypeException("Response type must be Result<> or Result");
		}

#pragma warning disable S3011
		var result = Activator.CreateInstance(responseType, BindingFlags.Instance | BindingFlags.NonPublic, null, [false, error], null);
#pragma warning restore S3011
		ArgumentNullException.ThrowIfNull(result);

		return (TResponse)result;
	}
}
