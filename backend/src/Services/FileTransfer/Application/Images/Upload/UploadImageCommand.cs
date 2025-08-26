using Common.Application.Abstractions.Command;
using SharedKernel;

namespace FileTransfer.Application.Images.Upload;

public sealed record UploadImageCommand(Stream Stream, string MediaType, FileName FileName, FileLength FileLength)
	: ITransactionalCommand<UploadImageResponse>;
