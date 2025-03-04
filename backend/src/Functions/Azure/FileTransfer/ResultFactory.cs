using Common.Presentation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace FileTransfer;

public static class ResultFactory
{
	public static BadRequestObjectResult CreateBadRequestResult(string detail) =>
		new(new ProblemDetails
		{
			Title  = "Bad Request",
			Status = StatusCodes.Status400BadRequest,
			Detail = detail
		});

	public static BadRequestObjectResult CreateInternalErrorResult() =>
		new(new ProblemDetails
		{
			Title  = "Internal Server Error",
			Status = StatusCodes.Status500InternalServerError
		});

	public static BadRequestObjectResult CreateValidationProblemResult(IDictionary<string, string[]> errors) =>
		new(new ProblemDetails
		{
			Type   = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
			Title  = "One or more validation errors occurred.",
			Status = StatusCodes.Status400BadRequest,
			Extensions = new Dictionary<string, object?>
			{
				{ ProblemDetailsExtensionKeys.Errors, errors },
				{ ProblemDetailsExtensionKeys.ErrorCode, ErrorCodes.ValidationFailed }
			}
		});
}
