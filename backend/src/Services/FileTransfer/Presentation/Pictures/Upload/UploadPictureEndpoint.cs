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

namespace Presentation.Pictures.Upload;

internal static class UploadPictureEndpoint
{
	public static void MapUploadPictureEndpoint(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapPost("",
				async ([FromForm] UploadPictureRequest? request, IValidator<UploadPictureRequest> validator, ISender sender, HttpContext httpContext) =>
				{
					if (request is null)
					{
						return Results.BadRequest();
					}

					var validationResult = await validator.ValidateAsync(request);
					if (!validationResult.IsValid)
					{
						return Results.ValidationProblem(validationResult.ToDictionary());
					}

					ArgumentNullException.ThrowIfNull(request.File);

					var mediaType = new MediaTypeHeaderValue(request.File.ContentType).MediaType;
					var command   = new UploadPictureCommand(request.File.OpenReadStream(), mediaType ?? "", request.File.FileName, request.File.Length);
					var result    = await sender.Send(command);
					httpContext.MaybeAddError(result);

					return result.Match(response => Results.Created($"/pictures/{response.Id}", response.Id),
						error => error switch
						{
							ValidationError valError => Results.ValidationProblem(valError.Errors),
							_                        => Results.InternalServerError()
						});
				})
			.DisableAntiforgery()
			.WithTags(EndpointTags.Pictures);
	}
}
