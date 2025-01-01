using System.Diagnostics;
using Application.Abstractions;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

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
		var elapsedMs = Math.Floor(Stopwatch.GetElapsedTime(startTime).TotalMilliseconds);

		var status = result.IsSuccess ? "successfully" : "failing";
		_logger.LogInformation("Request '{Name}' finished in {Time}ms {Status}", typeof(TRequest).Name, elapsedMs, status);

		return result;
	}
}
