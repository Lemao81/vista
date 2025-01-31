using System.Net.Http.Headers;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Presentation.Pictures.Upload;

internal sealed class FormFileValidator : AbstractValidator<IFormFile?>
{
	public FormFileValidator()
	{
		RuleFor(f => f!.ContentType)
			.NotNull()
			.NotEmpty()
			.Must(c => MediaTypeHeaderValue.TryParse(c, out _))
			.WithMessage("'Content Type' not valid.")
			.When(f => f is not null);

		RuleFor(f => f!.FileName).NotNull().NotEmpty().Must(ContainExtension).WithMessage("'File Name' must have an extension.").When(f => f is not null);

		RuleFor(f => f!.Length).GreaterThan(0).When(f => f is not null);
	}

	private static bool ContainExtension(string fileName) => fileName.Contains('.', StringComparison.OrdinalIgnoreCase) && fileName.Last() != '.';
}
