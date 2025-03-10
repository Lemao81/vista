using FileTransfer.Domain.ValueObjects;
using SharedKernel;

namespace Service.Tests.FileTransfer;

public class StorageObjectNameTests
{
	[Fact]
	public void When_StorageObjectName_given_string_path_parts_should_create_object_name_with_paths()
	{
		// Arrange
		var fileName = new FileName("Test.txt");

		// Act
		var objectName = new StorageObjectName(fileName, "path1", "path2");

		// Assert
		Assert.Equal("/path1/path2/test.txt", objectName.Value);
	}

	[Fact]
	public void When_StorageObjectName_given_non_string_path_parts_should_create_object_name_with_paths()
	{
		// Arrange
		var fileName = new FileName("Test.txt");

		// Act
		var objectName = new StorageObjectName(fileName, Guid.Parse("8f36bfd6-3f03-44f7-848c-795c6834faad"), 234);

		// Assert
		Assert.Equal("/8f36bfd6-3f03-44f7-848c-795c6834faad/234/test.txt", objectName.Value);
	}

	[Fact]
	public void When_StorageObjectName_given_no_parts_should_create_object_name_with_file_name()
	{
		// Arrange
		var fileName = new FileName("Test.txt");

		// Act
		var objectName = new StorageObjectName(fileName);

		// Assert
		Assert.Equal("/test.txt", objectName.Value);
	}
}
