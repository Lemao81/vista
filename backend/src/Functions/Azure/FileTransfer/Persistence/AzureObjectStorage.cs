using Azure.Storage.Blobs;
using FileTransfer.Domain.Media;
using FileTransfer.Domain.Models;
using FileTransfer.Domain.ValueObjects;
using Lemao.UtilExtensions;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace FileTransfer.Persistence;

public class AzureObjectStorage : IObjectStorage
{
	private readonly BlobServiceClient           _blobServiceClient;
	private readonly ILogger<AzureObjectStorage> _logger;

	public AzureObjectStorage(BlobServiceClient blobServiceClient, ILogger<AzureObjectStorage> logger)
	{
		_blobServiceClient = blobServiceClient;
		_logger            = logger;
	}

	public async Task<Result> CreateBucketAsync(StorageBucket bucket, CancellationToken cancellationToken = default)
	{
		try
		{
			var response = await _blobServiceClient.CreateBlobContainerAsync(StorageBucket.Media.Value, cancellationToken: cancellationToken);

			return response.HasValue ? Result.Success() : Errors.StorageBucketCreationRequestFailed;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception);

			return Errors.StorageBucketCreationFailed;
		}
	}

	public async Task<Result> PutObjectAsync(
		StorageBucket     bucket,
		StorageObjectName objectName,
		string?           contentType,
		Stream            stream,
		CancellationToken cancellationToken = default)
	{
		try
		{
			var blobContainerClient = _blobServiceClient.GetBlobContainerClient(StorageBucket.Media.Value);
			var blobClient          = blobContainerClient.GetBlobClient(objectName.Value);
			var response            = await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);

			return response.HasValue ? Result.Success() : Errors.ObjectStorageRequestFailed;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception);

			return Errors.ObjectStorageFailed;
		}
	}

	public Task<Result<List<StorageItem>>> GetObjectItemsAsync(StorageBucket bucket, StoragePrefix prefix, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<Result<Stream>> GetObjectAsync(StorageBucket bucket, StorageObjectName objectName, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<Result<bool>> ObjectExistAsync(StorageBucket bucket, StorageObjectName objectName, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}
