using FileTransfer.Domain.Media;
using SharedKernel;

namespace FileTransfer.Application.Utilities;

public static class ApplicationHelper
{
	public static FileName GetStorageFileName(FileName fileName, MediaItem mediaItem) =>
		new($"{mediaItem.Id.Value}.{ToFileNameSuffix(mediaItem.MediaSizeKind)}.{fileName.Extension}");

	private static string ToFileNameSuffix(MediaSizeKind sizeKind) =>
		sizeKind switch
		{
			MediaSizeKind.Original => "orig",
			MediaSizeKind.Medium   => "medium",
			_                      => throw new ArgumentOutOfRangeException(nameof(sizeKind), sizeKind, null),
		};
}
