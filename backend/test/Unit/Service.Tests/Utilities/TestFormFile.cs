using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Service.Tests.Utilities;

public class TestFormFile : IFormFile
{
	public TestFormFile(string contentType = "", string contentDisposition = "", long length = 0, string name = "", string fileName = "")
	{
		ContentType        = contentType;
		ContentDisposition = contentDisposition;
		Headers            = Substitute.For<IHeaderDictionary>();
		Length             = length;
		Name               = name;
		FileName           = fileName;
	}

	public Stream OpenReadStream() => throw new NotImplementedException();

	public void CopyTo(Stream target) => throw new NotImplementedException();

	public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new()) => throw new NotImplementedException();

	public string            ContentType        { get; }
	public string            ContentDisposition { get; }
	public IHeaderDictionary Headers            { get; }
	public long              Length             { get; }
	public string            Name               { get; }
	public string            FileName           { get; }
}
