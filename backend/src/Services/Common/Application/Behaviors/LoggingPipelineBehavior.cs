using System.Diagnostics;
using Common.Application.Abstractions;
using Common.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SharedKernel;

namespace Common.Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
	where TResponse : Result
{
	private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

	public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var startTime = Stopwatch.GetTimestamp();
		var result    = await next(cancellationToken);
		var elapsedMs = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;

		if (result.IsSuccess)
		{
			_logger.MediatRRequestFinished(typeof(TRequest).Name, isSuccess: true, elapsedMs);
		}
		else
		{
			using (LogContext.PushProperty("Error", result.Error, destructureObjects: true))
			{
				_logger.MediatRRequestFinished(typeof(TRequest).Name, isSuccess: false, elapsedMs);
			}
		}

		return result;
	}
}
