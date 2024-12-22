using FluentValidation;

namespace WebApi.Pictures.Upload;

public sealed class UploadPictureRequestValidator : AbstractValidator<UploadPictureRequest>
{
	public UploadPictureRequestValidator()
	{
		RuleFor(r => r.File)
			.NotNull()
			.DependentRules(() =>
				RuleFor(r => r.File)
					.Must(f => f is not null && f.FileName.Contains('.') && f.Name.Last() != '.')
					.WithMessage("File name must have an extension."));
	}
}
