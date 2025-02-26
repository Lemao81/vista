using System.Net.Http.Headers;
using Application.Pictures.Upload;
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

					var mediaType = new MediaTypeHeaderValue(request.File.ContentType).MediaType;
					var command   = new UploadPictureCommand(request.File.OpenReadStream(), mediaType ?? "", request.File.FileName, request.File.Length);
					var result    = await sender.Send(command);
					httpContext.MaybeAddError(result);

					return result.Match(response => Created($"/pictures/{response.Id}", response.Id),
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
