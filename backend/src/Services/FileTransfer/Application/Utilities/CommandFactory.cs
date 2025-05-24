using System.Net.Http.Headers;
using FileTransfer.Application.Pictures.Upload;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.Application.Utilities;

public static class CommandFactory
{
	public static UploadPictureCommand CreateUploadPictureCommand(IFormFile file)
	{
		var mediaType = new MediaTypeHeaderValue(file.ContentType).MediaType;

		return new UploadPictureCommand(file.OpenReadStream(), mediaType ?? string.Empty, file.FileName, file.Length);
	}
}
