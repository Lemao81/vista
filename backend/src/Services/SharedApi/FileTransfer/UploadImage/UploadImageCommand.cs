using Common.Application.Abstractions.Command;
using SharedKernel;

namespace SharedApi.FileTransfer.UploadImage;

public sealed record UploadImageCommand(Stream Stream, string MediaType, FileName FileName, FileLength FileLength)
	: ITransactionalCommand<UploadImageResponse>;
