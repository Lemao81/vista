using Domain.Media;
using FluentValidation;
using Microsoft.Extensions.Options;
using Lemao.UtilExtensions;

namespace Application.Pictures.Upload;

public class UploadPictureCommandValidator : AbstractValidator<UploadPictureCommand>
{
	public UploadPictureCommandValidator(IOptions<UploadMediaOptions> options)
	{
		var uploadOptions = options.Value;

		RuleFor(c => c.MediaType)
			.Must(v => uploadOptions.ValidPictureContentTypes.Contains(v))
			.WithMessage($"'Content Type' must be one of: {uploadOptions.ValidPictureContentTypes.ToCommaSeparated()}");

		RuleFor(c => c.FileName)
			.Must(f => uploadOptions.ValidPictureFileExtensions.Contains(f.Extension))
			.WithMessage($"'File Name' must have an extension of: {uploadOptions.ValidPictureFileExtensions.ToCommaSeparated()}");

		RuleFor(c => c.FileLength).GreaterThan(0);
		RuleFor(c => c.FileLength).LessThanOrEqualTo(uploadOptions.MaxPictureFileLengthKb * 1024);
	}
}
