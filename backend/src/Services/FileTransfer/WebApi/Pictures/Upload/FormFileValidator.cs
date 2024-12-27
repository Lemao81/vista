﻿using FluentValidation;

namespace WebApi.Pictures.Upload;

internal sealed class FormFileValidator : AbstractValidator<IFormFile?>
{
	public FormFileValidator()
	{
		RuleFor(f => f!.FileName)
			.NotNull()
			.NotEmpty()
			.Must(ContainExtension)
			.WithMessage("File name must have an extension.")
			.When(f => f is not null);
		RuleFor(f => f!.Length).GreaterThan(0).When(f => f is not null);
	}

	private static bool ContainExtension(string fileName) => fileName.Contains('.') && fileName.Last() != '.';
}