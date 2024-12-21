using MediatR;

namespace Application.Pictures.Upload;

internal sealed class UploadPictureHandler : IRequestHandler<UploadPictureCommand, UploadPictureResponse>
{
    public async Task<UploadPictureResponse> Handle(UploadPictureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return new UploadPictureResponse(Guid.NewGuid());
        }
        finally
        {
            await request.Stream.DisposeAsync();
        }
    }
}
