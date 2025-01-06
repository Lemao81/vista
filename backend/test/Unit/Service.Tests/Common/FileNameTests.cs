using Domain.ValueObjects;

namespace Service.Tests.Common;

public class FileNameTests
{
	[Theory]
	[InlineData("pic.png", "pic", "png")]
	[InlineData("img.jpeg", "img", "jpeg")]
	public void When_new_given_valid_file_name_should_set_values(string value, string expectedName, string expectedExtension)
	{
		// Act
		var fileName = new FileName(value);

		// Assert
		Assert.Equal(value, fileName.Value);
		Assert.Equal(expectedName, fileName.Name);
		Assert.Equal(expectedExtension, fileName.Extension);
	}

	[Theory]
	[InlineData("pic.", typeof(ArgumentException))]
	[InlineData("pic", typeof(ArgumentException))]
	public void When_new_given_invalid_file_name_should_throw(string value, Type expectedExceptionType)
	{
		// Act + Assert
		Assert.Throws(expectedExceptionType, () => new FileName(value));
	}
}
