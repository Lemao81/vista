using FileTransfer.Domain.Media;
using FluentValidation;
using Lemao.UtilExtensions;
using SharedApi.FileTransfer.UploadImage;

namespace FileTransfer.Application.Images.Upload;

public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
	public UploadImageCommandValidator(UploadMediaOptions options)
	{
		RuleFor(c => c.MediaType)
			.Must(v => options.ValidImageContentTypes.Contains(v, StringComparer.Ordinal))
			.WithMessage($"'Content Type' must be one of: {options.ValidImageContentTypes.ToCommaSeparated()}");

		RuleFor(c => c.FileName)
			.Must(f => options.ValidImageFileExtensions.Contains(f.Extension, StringComparer.Ordinal))
			.WithMessage($"'File Name' must have an extension of: {options.ValidImageFileExtensions.ToCommaSeparated()}");

		RuleFor(c => c.FileLength).GreaterThan(0);
		RuleFor(c => c.FileLength).LessThanOrEqualTo(options.MaxImageFileLengthKb * 1024);
	}
}
