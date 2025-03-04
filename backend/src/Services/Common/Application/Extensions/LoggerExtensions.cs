using Microsoft.Extensions.Logging;

namespace Common.Application.Extensions;

public static class LoggerExtensions
{
	private const int MediatRRequestFinishedId = 0;

	private static readonly Action<ILogger, string, string, double, Exception?> MediatRRequestFinishedAction =
		LoggerMessage.Define<string, string, double>(LogLevel.Information,
			new EventId(MediatRRequestFinishedId, nameof(MediatRRequestFinished)),
			"MediatR Request finished {RequestName} - {Status} in {RequestTime} ms");

	public static void MediatRRequestFinished(this ILogger logger, string requestName, bool isSuccess, double milliseconds) =>
		MediatRRequestFinishedAction(logger, requestName, isSuccess ? "Success" : "Failure", milliseconds, null);
}
