using System.ComponentModel.DataAnnotations;

namespace Domain.Media;

public class UploadMediaOptions
{
	[Required]
	public required IEnumerable<string> ValidPictureContentTypes { get; set; }

	[Required]
	public required IEnumerable<string> ValidPictureFileExtensions { get; set; }

	[Range(1, int.MaxValue)]
	public int MaxPictureFileLengthKb { get; set; }
}
