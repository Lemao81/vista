using Application.Pictures.Upload;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Constants;

namespace WebApi.Pictures.Upload;

public static class UploadPictureEndpoint
{
    public static void MapUploadPictureEndpoint(this RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost("", async ([FromForm] UploadPictureRequest? request, IValidator<UploadPictureRequest> validator, ISender sender) =>
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
            var response = await sender.Send(command);

            return Results.Created($"/pictures/{response.Id}", response.Id);
        }).DisableAntiforgery().WithTags(EndpointTags.Pictures);
    }
}
