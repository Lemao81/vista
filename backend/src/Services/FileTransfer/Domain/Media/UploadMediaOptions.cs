using System.ComponentModel.DataAnnotations;

namespace FileTransfer.Domain.Media;

public class UploadMediaOptions
{
	[Required]
	public required IEnumerable<string> ValidImageContentTypes { get; set; }

	[Required]
	public required IEnumerable<string> ValidImageFileExtensions { get; set; }

	[Range(1, int.MaxValue)]
	public int MaxImageFileLengthKb { get; set; }
}
