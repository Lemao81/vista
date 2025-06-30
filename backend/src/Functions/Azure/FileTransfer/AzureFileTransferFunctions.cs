﻿using Common.Presentation.Constants;
using FileTransfer.Application.Utilities;
using FluentValidation;
using Lemao.UtilExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SharedKernel;
using static FileTransfer.ResultFactory;

namespace FileTransfer;

public class AzureFileTransferFunctions
{
	private readonly ILogger<AzureFileTransferFunctions> _logger;
	private readonly IValidator<IFormFile?>              _formFileValidator;
	private readonly ISender                             _sender;

	public AzureFileTransferFunctions(ILogger<AzureFileTransferFunctions> logger, IValidator<IFormFile?> formFileValidator, ISender sender)
	{
		_logger            = logger;
		_formFileValidator = formFileValidator;
		_sender            = sender;
	}

	[Function("Health")]
	public IActionResult Health([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.Health)] HttpRequest req, FunctionContext ctx)
	{
		var reqLogger = ctx.GetLogger<AzureFileTransferFunctions>();
		reqLogger.LogInformation("Ctx Logger Information");
		reqLogger.LogWarning("Ctx Logger Warning");
		reqLogger.LogError(new InvalidOperationException("just for the testing"), "Ctx Logger {Error}", "Error");
		_logger.LogInformation("Calling Health function");
		_logger.LogWarning("Calling Health function with warning");
		_logger.LogError(new MissingConfigurationException("just for testing"), "Calling Health function with error | {Error}", "the error text");
		Console.WriteLine("Some console logging");

		return new OkObjectResult(HealthStatus.Healthy.ToString());
	}

	[Function("UploadPicture")]
	public async Task<IActionResult> UploadPictureAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = Routes.Pictures)] HttpRequest req)
	{
		_logger.LogInformation("Calling UploadPicture function");

		try
		{
			IFormCollection formData;
			try
			{
				formData = await req.ReadFormAsync();
			}
			catch (InvalidOperationException exception)
			{
				return CreateBadRequestResult(exception.Message);
			}

			if (formData.Files.Count == 0)
			{
				return CreateBadRequestResult("No files attached");
			}

			var validationResult = await _formFileValidator.ValidateAsync(formData.Files[0]);
			if (!validationResult.IsValid)
			{
				return CreateValidationProblemResult(validationResult.ToDictionary());
			}

			var command = CommandFactory.CreateUploadPictureCommand(formData.Files[0]);
			var result  = await _sender.Send(command);

			//// TODO add function equivalent
			//// httpContext.MaybeAddError(result);

			return result.Match<IActionResult>(
				response => new CreatedResult($"/{Routes.Pictures}/{response.Id}", response.Id),
				error => error switch
				{
					ValidationError valError => CreateValidationProblemResult(valError.Errors),
					_                        => CreateInternalErrorResult(),
				});
		}
		catch (Exception exception)
		{
			_logger.LogError(exception);

			return CreateInternalErrorResult();
		}
	}
}
