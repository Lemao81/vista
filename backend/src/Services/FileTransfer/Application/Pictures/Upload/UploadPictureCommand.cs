using Common.Application.Abstractions;
using SharedKernel;

namespace FileTransfer.Application.Pictures.Upload;

public sealed record UploadPictureCommand(Stream Stream, string MediaType, FileName FileName, FileLength FileLength)
	: ITransactionalCommand<UploadPictureResponse>;
