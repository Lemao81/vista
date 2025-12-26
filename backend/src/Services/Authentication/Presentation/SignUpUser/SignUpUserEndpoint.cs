using Common.Presentation.Constants;
using Common.Presentation.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedApi.Authentication;
using SharedApi.Authentication.SignUpUser;
using SharedKernel;
using static Microsoft.AspNetCore.Http.Results;

namespace Authentication.Presentation.SignUpUser;

public static class SignUpUserEndpoint
{
	public static void MapSignUpUser(this RouteGroupBuilder groupBuilder)
	{
		groupBuilder.MapPost(
				"/signUp",
				async (SignUpUserRequest request, IValidator<SignUpUserRequest> requestValidator, ISender sender, HttpContext httpContext) =>
				{
					var validationResult = await requestValidator.ValidateAsync(request, httpContext.RequestAborted);
					if (!validationResult.IsValid)
					{
						return ValidationProblem(validationResult.ToDictionary());
					}

					var command = CommandFactory.CreateSignUpUserCommand(request);
					var result  = await sender.Send(command, httpContext.RequestAborted);
					httpContext.MaybeAddError(result);

					return result.Match(
						() => Ok(),
						error => error switch
						{
							ValidationError valError => ValidationProblem(valError.Errors),
							_                        => InternalServerError(),
						});
				})
			.DisableAntiforgery()
			.WithTags(EndpointTags.Auth)
			.AllowAnonymous();
	}
}
