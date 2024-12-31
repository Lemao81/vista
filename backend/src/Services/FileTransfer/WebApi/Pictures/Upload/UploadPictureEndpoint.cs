using Application.Pictures.Upload;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Constants;

namespace WebApi.Pictures.Upload;

public static class UploadPictureEndpoint
{
	public static void MapUploadPictureEndpoint(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapPost("",
				async ([FromForm] UploadPictureRequest? request, IValidator<UploadPictureRequest> validator, ISender sender) =>
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

					var command = new UploadPictureCommand(request.File.OpenReadStream(), request.File.FileName, request.File.Length);
					var result  = await sender.Send(command);

					return result.Match(response => Results.Created($"/pictures/{response.Id}", response.Id),
						error => error switch
						{
							ValidationError validationError => Results.ValidationProblem(validationError.Errors),
							_                               => Results.InternalServerError()
						});
				})
			.DisableAntiforgery()
			.WithTags(EndpointTags.Pictures);
	}
}
