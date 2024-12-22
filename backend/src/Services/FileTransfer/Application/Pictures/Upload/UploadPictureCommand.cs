using Domain.ValueObjects;
using MediatR;

namespace Application.Pictures.Upload;

public sealed record UploadPictureCommand(Stream Stream, FileName FileName, FileLength FileLength) : IRequest<UploadPictureResponse>;
