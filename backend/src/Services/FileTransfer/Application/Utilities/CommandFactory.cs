using System.Net.Http.Headers;
using Application.Pictures.Upload;
using Microsoft.AspNetCore.Http;

namespace Application.Utilities;

public static class CommandFactory
{
	public static UploadPictureCommand CreateUploadPictureCommand(IFormFile file)
	{
		var mediaType = new MediaTypeHeaderValue(file.ContentType).MediaType;

		return new UploadPictureCommand(file.OpenReadStream(), mediaType ?? "", file.FileName, file.Length);
	}
}
