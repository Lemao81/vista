using System.Diagnostics.Metrics;

namespace Application.Media;

public class FileTransferMetrics
{
	private readonly Counter<int> _pictureUploadCounter;

	public FileTransferMetrics(IMeterFactory meterFactory)
	{
		using var meter = meterFactory.Create(MeterNames.FileTransfer);
		_pictureUploadCounter = meter.CreateCounter<int>(CounterNames.PictureUpload);
	}

	public void PictureUploaded(string mediaType)
	{
		_pictureUploadCounter.Add(1, new KeyValuePair<string, object?>(CounterTagNames.PictureMediaType, mediaType));
	}
}
