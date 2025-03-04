using Common.Domain.Storage;
using Minio;
using Minio.DataModel.Args;

namespace WebApi.Initiators;

internal sealed class MinioInitiator : IInitiator
{
	private readonly IMinioClient            _minioClient;
	private readonly ILogger<MinioInitiator> _logger;

	public MinioInitiator(IMinioClient minioClient, ILogger<MinioInitiator> logger)
	{
		_minioClient = minioClient;
		_logger      = logger;
	}

	public async Task<bool> InitiateAsync(CancellationToken cancellationToken = default)
	{
		var result = await CreateBucketAsync(Buckets.Media, cancellationToken);

		return result;
	}

	private async Task<bool> CreateBucketAsync(string bucket, CancellationToken cancellationToken)
	{
		var existsArgs = new BucketExistsArgs().WithBucket(bucket);
		var exists     = await _minioClient.BucketExistsAsync(existsArgs, cancellationToken);
		if (exists)
		{
			return true;
		}

		var makeArgs = new MakeBucketArgs().WithBucket(bucket);
		await _minioClient.MakeBucketAsync(makeArgs, cancellationToken);

		_logger.LogInformation("Minio bucket '{Bucket}' created", bucket);

		return true;
	}
}
