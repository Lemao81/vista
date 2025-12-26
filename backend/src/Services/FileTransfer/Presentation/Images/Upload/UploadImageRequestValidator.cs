using FluentValidation;
using Microsoft.AspNetCore.Http;
using SharedApi.FileTransfer.UploadImage;

namespace FileTransfer.Presentation.Images.Upload;

internal sealed class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
	public UploadImageRequestValidator(IValidator<IFormFile?> formFileValidator)
	{
		RuleFor(r => r.File).NotNull().SetValidator(formFileValidator);
	}
}
