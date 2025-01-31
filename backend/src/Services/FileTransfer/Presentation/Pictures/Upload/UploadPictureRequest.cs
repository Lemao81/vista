using Microsoft.AspNetCore.Http;

namespace Presentation.Pictures.Upload;

internal sealed record UploadPictureRequest(IFormFile? File);
