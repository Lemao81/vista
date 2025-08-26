using System.Diagnostics.Metrics;
using Common.Application.Constants;

namespace FileTransfer.Application.Media;

public class FileTransferMetrics
{
	private readonly Counter<int> _imageUploadCounter;

	public FileTransferMetrics(IMeterFactory meterFactory)
	{
		using var meter = meterFactory.Create(MeterNames.FileTransfer);
		_imageUploadCounter = meter.CreateCounter<int>(CounterNames.ImageUpload);
	}

	public void ImageUploaded(string mediaType)
	{
		_imageUploadCounter.Add(1, new KeyValuePair<string, object?>(CounterTagNames.ImageMediaType, mediaType));
	}
}
