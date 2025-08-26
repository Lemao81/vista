using Common.Presentation.Constants;
using FileTransfer.Application.Utilities;
using FileTransfer.Presentation.Constants;
using FileTransfer.Presentation.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using static Microsoft.AspNetCore.Http.Results;

namespace FileTransfer.Presentation.Images.Upload;

internal static class UploadImageEndpoint
{
	public static void MapUploadImageEndpoint(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapPost(
				string.Empty,
				async ([FromForm] UploadImageRequest? request, IValidator<UploadImageRequest> requestValidator, ISender sender, HttpContext httpContext) =>
				{
					if (request is null)
					{
						return BadRequest();
					}

					var validationResult = await requestValidator.ValidateAsync(request, httpContext.RequestAborted);
					if (!validationResult.IsValid)
					{
						return ValidationProblem(validationResult.ToDictionary());
					}

					ArgumentNullException.ThrowIfNull(request.File);

					var command = CommandFactory.CreateUploadImageCommand(request.File);
					var result  = await sender.Send(command, httpContext.RequestAborted);
					httpContext.MaybeAddError(result);

					return result.Match(
						response => Created($"/{Routes.Images}/{response.Id}", response.Id),
						error => error switch
						{
							ValidationError valError => ValidationProblem(valError.Errors),
							_                        => InternalServerError(),
						});
				})
			.DisableAntiforgery()
			.WithTags(EndpointTags.Images);
	}
}
