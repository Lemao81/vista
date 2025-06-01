using FileTransfer.Domain.Media;
using FluentValidation;
using Lemao.UtilExtensions;

namespace FileTransfer.Application.Pictures.Upload;

public class UploadPictureCommandValidator : AbstractValidator<UploadPictureCommand>
{
	public UploadPictureCommandValidator(UploadMediaOptions options)
	{
		RuleFor(c => c.MediaType)
			.Must(v => options.ValidPictureContentTypes.Contains(v, StringComparer.Ordinal))
			.WithMessage($"'Content Type' must be one of: {options.ValidPictureContentTypes.ToCommaSeparated()}");

		RuleFor(c => c.FileName)
			.Must(f => options.ValidPictureFileExtensions.Contains(f.Extension, StringComparer.Ordinal))
			.WithMessage($"'File Name' must have an extension of: {options.ValidPictureFileExtensions.ToCommaSeparated()}");

		RuleFor(c => c.FileLength).GreaterThan(0);
		RuleFor(c => c.FileLength).LessThanOrEqualTo(options.MaxPictureFileLengthKb * 1024);
	}
}
