using Common.Presentation.Constants;
using FileTransfer.Presentation.Images.Upload;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace FileTransfer.Presentation.Images;

public static class ImagesEndpointGroup
{
	public static void MapImageEndpoints(this RouteGroupBuilder apiGroup)
	{
		var imagesGroup = apiGroup.MapGroup(Routes.Images);
		imagesGroup.MapUploadImageEndpoint();
	}
}
