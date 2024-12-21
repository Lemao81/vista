using FluentValidation;

namespace WebApi.Pictures.Upload;

public sealed class UploadPictureRequestValidator : AbstractValidator<UploadPictureRequest>
{
	public UploadPictureRequestValidator()
	{
		RuleFor(r => r.File).NotNull();
	}
}
