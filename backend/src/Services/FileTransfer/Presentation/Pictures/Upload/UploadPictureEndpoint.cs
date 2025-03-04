using Application.Utilities;
using Common.Presentation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using SharedKernel;
using static Microsoft.AspNetCore.Http.Results;

namespace Presentation.Pictures.Upload;

internal static class UploadPictureEndpoint
{
	public static void MapUploadPictureEndpoint(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapPost("",
				async ([FromForm] UploadPictureRequest? request, IValidator<UploadPictureRequest> requestValidator, ISender sender, HttpContext httpContext) =>
				{
					if (request is null)
					{
						return BadRequest();
					}

					var validationResult = await requestValidator.ValidateAsync(request);
					if (!validationResult.IsValid)
					{
						return ValidationProblem(validationResult.ToDictionary());
					}

					ArgumentNullException.ThrowIfNull(request.File);

					var command = CommandFactory.CreateUploadPictureCommand(request.File);
					var result  = await sender.Send(command);
					httpContext.MaybeAddError(result);

					return result.Match(response => Created($"/{Routes.Pictures}/{response.Id}", response.Id),
						error => error switch
						{
							ValidationError valError => ValidationProblem(valError.Errors),
							_                        => InternalServerError()
						});
				})
			.DisableAntiforgery()
			.WithTags(EndpointTags.Pictures);
	}
}
