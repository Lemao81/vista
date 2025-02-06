﻿using System.Diagnostics.CodeAnalysis;
using DbUp.Engine.Output;

namespace WebApi;

[SuppressMessage("Usage", "CA2254:Template should be a static expression")]
public class DbUpLogger : IUpgradeLog
{
	private readonly ILogger _logger;

	public DbUpLogger(ILogger logger)
	{
		_logger = logger;
	}

	public void LogTrace(string format, params object[] args) => _logger.LogTrace(format, args);

	public void LogDebug(string format, params object[] args) => _logger.LogDebug(format, args);

	public void LogInformation(string format, params object[] args) => _logger.LogInformation(format, args);

	public void LogWarning(string format, params object[] args) => _logger.LogWarning(format, args);

	public void LogError(string format, params object[] args) => _logger.LogError(format, args);

	public void LogError(Exception ex, string format, params object[] args) => _logger.LogError(ex, format, args);
}
