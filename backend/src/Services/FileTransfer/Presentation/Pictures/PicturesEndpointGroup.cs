using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Presentation.Pictures.Upload;

namespace Presentation.Pictures;

public static class PicturesEndpointGroup
{
	public static void MapPictureEndpoints(this RouteGroupBuilder apiGroup)
	{
		var picturesGroup = apiGroup.MapGroup(Routes.Pictures);
		picturesGroup.MapUploadPictureEndpoint();
	}
}
