using SharedKernel;

namespace Service.Tests.Tests.Common;

public class FileNameTests
{
	[Theory]
	[InlineData("Pic.pNg", "Pic", "pic.png", "png")]
	[InlineData("imG.jpeG", "imG", "img.jpeg", "jpeg")]
	public void NewValidFileName_Should_SetValues(string value, string expectedBaseName, string expectedNormalizedName, string expectedExtension)
	{
		// Act
		var fileName = new FileName(value);

		// Assert
		Assert.Equal(value, fileName.Value);
		Assert.Equal(expectedBaseName, fileName.BaseName);
		Assert.Equal(expectedNormalizedName, fileName.NormalizedValue);
		Assert.Equal(expectedExtension, fileName.Extension);
	}

	[Theory]
	[InlineData("pic.", typeof(ArgumentException))]
	[InlineData("pic", typeof(ArgumentException))]
	public void NewInvalidFileName_Should_Throw(string value, Type expectedExceptionType)
	{
		// Act + Assert
		Assert.Throws(expectedExceptionType, () => new FileName(value));
	}
}
