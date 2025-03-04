using Common.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Common.Application.Behaviors;

public sealed class ExceptionHandlingPipelineBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> _logger;

	public ExceptionHandlingPipelineBehavior(ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger)
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
