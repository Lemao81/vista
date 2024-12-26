using FluentValidation;

namespace WebApi.Pictures.Upload;

internal sealed class UploadPictureRequestValidator : AbstractValidator<UploadPictureRequest>
{
	public UploadPictureRequestValidator()
	{
		RuleFor(r => r.File)
			.NotNull()
			.SetValidator(new FormFileValidator());
	}
}