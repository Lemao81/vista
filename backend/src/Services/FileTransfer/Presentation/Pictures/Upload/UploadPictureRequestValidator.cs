using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.Presentation.Pictures.Upload;

internal sealed class UploadPictureRequestValidator : AbstractValidator<UploadPictureRequest>
{
	public UploadPictureRequestValidator(IValidator<IFormFile?> formFileValidator)
	{
		RuleFor(r => r.File).NotNull().SetValidator(formFileValidator);
	}
}
