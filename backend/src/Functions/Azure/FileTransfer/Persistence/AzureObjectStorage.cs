using FileTransfer.Domain.Media;
using FileTransfer.Domain.Models;
using FileTransfer.Domain.ValueObjects;
using SharedKernel;

namespace FileTransfer.Persistence;

public class AzureObjectStorage : IObjectStorage
{
	public Task<Result> CreateBucketAsync(StorageBucket bucket, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<Result> PutObjectAsync(StorageBucket bucket, StorageObjectName objectName, string? contentType, Stream stream, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
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
