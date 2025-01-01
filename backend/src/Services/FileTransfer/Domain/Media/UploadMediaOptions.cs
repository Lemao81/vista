namespace Domain.Media;

public class UploadMediaOptions
{
	public required IEnumerable<string> ValidPictureContentTypes   { get; set; }
	public required IEnumerable<string> ValidPictureFileExtensions { get; set; }
	public          int                 MaxPictureFileLengthKb     { get; set; }
}
