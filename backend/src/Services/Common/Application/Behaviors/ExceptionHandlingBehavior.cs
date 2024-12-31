using Application.Abstractions;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class ExceptionHandlingBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

	public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		try
		{
			return await next();
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{Message}", exception.Message);

			return CreateErrorResult(Errors.Unknown);
		}
	}
}
