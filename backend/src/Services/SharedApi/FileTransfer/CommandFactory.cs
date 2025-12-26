using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using SharedApi.FileTransfer.UploadImage;

namespace SharedApi.FileTransfer;

public static class CommandFactory
{
	public static UploadImageCommand CreateUploadImageCommand(IFormFile file)
	{
		var mediaType = new MediaTypeHeaderValue(file.ContentType).MediaType;

		return new UploadImageCommand(file.OpenReadStream(), mediaType ?? string.Empty, file.FileName, file.Length);
	}
}
