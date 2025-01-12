using System.Diagnostics;
using Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SharedKernel;

namespace Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : BasePipelineBehavior<TRequest, TResponse> where TRequest : notnull where TResponse : Result
{
	private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

	public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var startTime = Stopwatch.GetTimestamp();
		var result    = await next();
		var elapsedMs = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;

		if (result.IsSuccess)
		{
			Log("Success", elapsedMs);
		}
		else
		{
			using (LogContext.PushProperty("Error", result.Error, true))
			{
				Log("Failure", elapsedMs);
			}
		}

		return result;
	}

	private void Log(string status, double elapsedMs) =>
		_logger.LogInformation("MediatR Request finished {RequestName} - {Status} in {RequestTime} ms", typeof(TRequest).Name, status, elapsedMs);
}
