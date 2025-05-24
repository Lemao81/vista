using System.Net;
using Azure.Storage.Blobs;
using Common.Domain.Constants.Storage;
using Common.WebApi.Constants;
using Lemao.UtilExtensions;
using Maintenance.WebApi.Abstractions;

namespace Maintenance.WebApi.Initiators;

public class AzureBlobStorageInitiator : IInitiator
{
	private readonly BlobServiceClient                  _blobServiceClient;
	private readonly ILogger<AzureBlobStorageInitiator> _logger;

	public AzureBlobStorageInitiator(BlobServiceClient blobServiceClient, ILogger<AzureBlobStorageInitiator> logger)
	{
		_blobServiceClient = blobServiceClient;
		_logger            = logger;
	}

	public bool IsEnabled() => EnvironmentVariable.IsTrue(EnvironmentVariableNames.InitiateAzureBlobStorage);

	public async Task<bool> InitiateAsync(CancellationToken cancellationToken = default)
	{
		var result = await CreateContainerAsync(Buckets.Media, cancellationToken);

		return result;
	}

	private async Task<bool> CreateContainerAsync(string containerName, CancellationToken cancellationToken)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
		if (await containerClient.ExistsAsync(cancellationToken))
		{
			_logger.LogInformation("Azure blob container '{Container}' exists", containerName);

			return true;
		}

		var response    = await containerClient.CreateAsync(cancellationToken: cancellationToken);
		var rawResponse = response.GetRawResponse();
		if (rawResponse.Status == (int)HttpStatusCode.Created)
		{
			_logger.LogInformation("Azure blob container '{Container}' created", containerName);

			return true;
		}

		_logger.LogWarning(
			"Azure blob container '{Container}' creation failed: {Status} - {Reason}",
			containerName,
			rawResponse.Status,
			rawResponse.ReasonPhrase);

		return false;
	}
}
