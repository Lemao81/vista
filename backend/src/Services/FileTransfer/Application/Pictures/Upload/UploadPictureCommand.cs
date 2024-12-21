using MediatR;

namespace Application.Pictures.Upload;

public sealed record UploadPictureCommand(Stream Stream, string FileName, long FileLength) : IRequest<UploadPictureResponse>;
