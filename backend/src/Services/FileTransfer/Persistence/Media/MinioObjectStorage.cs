using System.Net;
using FileTransfer.Domain.Media;
using FileTransfer.Domain.Models;
using FileTransfer.Domain.ValueObjects;
using Lemao.UtilExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SharedKernel;

namespace FileTransfer.Persistence.Media;

internal sealed class MinioObjectStorage : IObjectStorage
{
	private readonly IMinioClient                _minioClient;
	private readonly ILogger<MinioObjectStorage> _logger;

	public MinioObjectStorage(IMinioClient minioClient, ILogger<MinioObjectStorage> logger)
	{
		_minioClient = minioClient;
		_logger      = logger;
	}

	public async Task<Result> CreateBucketAsync(StorageBucket bucket, CancellationToken cancellationToken = default)
	{
		try
		{
			var makeArgs = new MakeBucketArgs().WithBucket(bucket.Value);
			await _minioClient.MakeBucketAsync(makeArgs, cancellationToken);

			return Result.Success();
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{Message}", exception.Message);

			return Errors.StorageBucketCreationFailed;
		}
	}

	public async Task<Result> PutObjectAsync(StorageBucket     bucket,
	                                         StorageObjectName objectName,
	                                         string?           contentType,
	                                         Stream            stream,
	                                         CancellationToken cancellationToken = default)
	{
		try
		{
			var putArgs = new PutObjectArgs().WithBucket(bucket.Value).WithObject(objectName.Value).WithObjectSize(stream.Length).WithStreamData(stream);
			if (!contentType.IsNullOrWhiteSpace())
			{
				putArgs.WithContentType(contentType);
			}

			var response = await _minioClient.PutObjectAsync(putArgs, cancellationToken);
			if (response.ResponseStatusCode == HttpStatusCode.OK)
			{
				return Result.Success();
			}

			_logger.LogError("Minio request failed: {ResponseStatusCode}", response.ResponseContent);

			return Errors.ObjectStorageRequestFailed;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{Message}", exception.Message);

			return Errors.ObjectStorageFailed;
		}
	}

	public async Task<Result<List<StorageItem>>> GetObjectItemsAsync(StorageBucket bucket, StoragePrefix prefix, CancellationToken cancellationToken = default)
	{
		try
		{
			var items    = new List<StorageItem>();
			var listArgs = new ListObjectsArgs().WithBucket(bucket.Value).WithPrefix(prefix.Value);
			await foreach (var item in _minioClient.ListObjectsEnumAsync(listArgs, cancellationToken))
			{
				if (item is not null)
				{
					items.Add(new StorageItem { Key = item.Key });
				}
			}

			return items;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{Message}", exception.Message);

			return Errors.ObjectListFailed;
		}
	}

	// TODO to be tested
	public async Task<Result<Stream>> GetObjectAsync(StorageBucket bucket, StorageObjectName objectName, CancellationToken cancellationToken = default)
	{
		try
		{
			var memoryStream = new MemoryStream();
			var getArgs = new GetObjectArgs().WithBucket(bucket.Value)
				.WithObject(objectName.Value)
				.WithCallbackStream(async (stream, token) => await stream.CopyToAsync(memoryStream, token));

			await _minioClient.GetObjectAsync(getArgs, cancellationToken);

			return memoryStream;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{Message}", exception.Message);

			return Errors.ObjectRetrievalFailed;
		}
	}

	public async Task<Result<bool>> ObjectExistAsync(StorageBucket bucket, StorageObjectName objectName, CancellationToken cancellationToken = default)
	{
		try
		{
			var statArgs = new StatObjectArgs().WithBucket(bucket.Value).WithObject(objectName.Value);
			await _minioClient.StatObjectAsync(statArgs, cancellationToken);

			return true;
		}
		catch (ObjectNotFoundException)
		{
			return false;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "{Message}", exception.Message);

			return Errors.ObjectExistFailed;
		}
	}
}
