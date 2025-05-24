using FileTransfer.Domain.Models;
using FileTransfer.Domain.ValueObjects;
using SharedKernel;

namespace FileTransfer.Domain.Media;

public interface IObjectStorage
{
	Task<Result> CreateBucketAsync(StorageBucket bucket, CancellationToken cancellationToken = default);

	Task<Result> PutObjectAsync(
		StorageBucket     bucket,
		StorageObjectName objectName,
		string?           contentType,
		Stream            stream,
		CancellationToken cancellationToken = default);

	Task<Result<List<StorageItem>>> GetObjectItemsAsync(StorageBucket bucket, StoragePrefix prefix, CancellationToken cancellationToken = default);

	Task<Result<Stream>> GetObjectAsync(StorageBucket bucket, StorageObjectName objectName, CancellationToken cancellationToken = default);

	Task<Result<bool>> ObjectExistAsync(StorageBucket bucket, StorageObjectName objectName, CancellationToken cancellationToken = default);
}
