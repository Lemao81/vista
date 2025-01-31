using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Presentation.Pictures.Upload;

namespace Presentation.Pictures;

public static class PicturesEndpointGroup
{
	public static void MapPictureEndpoints(this IEndpointRouteBuilder app)
	{
		var picturesGroup = app.MapGroup("pictures");
		picturesGroup.MapUploadPictureEndpoint();
	}
}
