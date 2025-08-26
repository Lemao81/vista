using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.Presentation.Images.Upload;

internal sealed class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
	public UploadImageRequestValidator(IValidator<IFormFile?> formFileValidator)
	{
		RuleFor(r => r.File).NotNull().SetValidator(formFileValidator);
	}
}
