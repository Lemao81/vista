using Application.Abstractions;
using Domain.ValueObjects;

namespace Application.Pictures.Upload;

public sealed record UploadPictureCommand(Stream Stream, FileName FileName, FileLength FileLength) : ICommand<UploadPictureResponse>;
