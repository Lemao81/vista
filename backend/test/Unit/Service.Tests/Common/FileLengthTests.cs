using SharedKernel;

namespace Service.Tests.Common;

public class FileLengthTests
{
	[Fact]
	public void New_Given_valid_length_Should_set_value()
	{
		// Act
		var fileLength = new FileLength(1000L);

		// Assert
		Assert.Equal(1000UL, fileLength.Value);
	}

	[Fact]
	public void New_Given_negative_length_Should_throw()
	{
		// Act + Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => new FileLength(-1L));
	}
}
