using Microsoft.AspNetCore.Http;

namespace FileTransfer.Presentation.Images.Upload;

internal sealed record UploadImageRequest(IFormFile? File);
