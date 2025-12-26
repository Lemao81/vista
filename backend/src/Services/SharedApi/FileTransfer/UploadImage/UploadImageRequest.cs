using Microsoft.AspNetCore.Http;

namespace SharedApi.FileTransfer.UploadImage;

public sealed record UploadImageRequest(IFormFile? File);
