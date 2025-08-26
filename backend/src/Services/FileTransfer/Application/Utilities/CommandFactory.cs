using System.Net.Http.Headers;
using FileTransfer.Application.Images.Upload;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.Application.Utilities;

public static class CommandFactory
{
	public static UploadImageCommand CreateUploadImageCommand(IFormFile file)
	{
		var mediaType = new MediaTypeHeaderValue(file.ContentType).MediaType;

		return new UploadImageCommand(file.OpenReadStream(), mediaType ?? string.Empty, file.FileName, file.Length);
	}
}
