using WebApi.Pictures.Upload;

namespace WebApi.Pictures;

public static class PicturesEndpointGroup
{
    public static void MapPictureEndpoints(this IEndpointRouteBuilder app)
    {
        var picturesGroup = app.MapGroup("pictures");
        picturesGroup.MapUploadPictureEndpoint();
    }
}
