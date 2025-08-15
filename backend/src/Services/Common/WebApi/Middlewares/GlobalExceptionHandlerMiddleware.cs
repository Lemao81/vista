using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.WebApi.Middlewares;

public class GlobalExceptionHandlerMiddleware : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
	private readonly IProblemDetailsService                    _problemDetailsService;

	public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger, IProblemDetailsService problemDetailsService)
	{
		_logger                = logger;
		_problemDetailsService = problemDetailsService;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, "{Message}", exception.Message);

		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

		return await _problemDetailsService.TryWriteAsync(
			       new ProblemDetailsContext
			       {
				       HttpContext = httpContext,
				       Exception   = exception,
				       ProblemDetails = new ProblemDetails
				       {
					       Type   = exception.GetType().FullName,
					       Title  = "Unhandled exception",
					       Detail = exception.Message,
					       Status = StatusCodes.Status500InternalServerError,
				       },
			       });
	}
}
