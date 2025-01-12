using System.Net.Http.Headers;
using Application.Abstractions;
using SharedKernel;

namespace Application.Pictures.Upload;

public sealed record UploadPictureCommand(Stream Stream, MediaTypeHeaderValue ContentType, FileName FileName, FileLength FileLength)
	: ITransactionalCommand<UploadPictureResponse>;
