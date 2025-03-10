using Common.Presentation;
using FileTransfer.Presentation.Pictures.Upload;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace FileTransfer.Presentation.Pictures;

public static class PicturesEndpointGroup
{
	public static void MapPictureEndpoints(this RouteGroupBuilder apiGroup)
	{
		var picturesGroup = apiGroup.MapGroup(Routes.Pictures);
		picturesGroup.MapUploadPictureEndpoint();
	}
}
