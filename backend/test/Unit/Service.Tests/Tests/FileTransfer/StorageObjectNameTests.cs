using FileTransfer.Domain.ValueObjects;
using SharedKernel;

namespace Service.Tests.Tests.FileTransfer;

public class StorageObjectNameTests
{
	[Fact]
	public void StringPathParts_Should_CreateWithPaths()
	{
		// Arrange
		var fileName = new FileName("Test.txt");

		// Act
		var objectName = new StorageObjectName(fileName, "path1", "path2");

		// Assert
		Assert.Equal("/path1/path2/test.txt", objectName.Value);
	}

	[Fact]
	public void NonStringPathParts_Should_CreateWithPaths()
	{
		// Arrange
		var fileName = new FileName("Test.txt");

		// Act
		var objectName = new StorageObjectName(fileName, Guid.Parse("8f36bfd6-3f03-44f7-848c-795c6834faad"), 234);

		// Assert
		Assert.Equal("/8f36bfd6-3f03-44f7-848c-795c6834faad/234/test.txt", objectName.Value);
	}

	[Fact]
	public void NoParts_Should_CreateWithFileName()
	{
		// Arrange
		var fileName = new FileName("Test.txt");

		// Act
		var objectName = new StorageObjectName(fileName);

		// Assert
		Assert.Equal("/test.txt", objectName.Value);
	}
}
