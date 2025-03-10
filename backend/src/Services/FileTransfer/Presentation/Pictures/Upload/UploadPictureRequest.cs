using Microsoft.AspNetCore.Http;

namespace FileTransfer.Presentation.Pictures.Upload;

internal sealed record UploadPictureRequest(IFormFile? File);
