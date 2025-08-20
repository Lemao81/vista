using SharedKernel;

namespace Service.Tests.Tests.Common;

public class FileLengthTests
{
	[Fact]
	public void NewValidLength_Should_SetValue()
	{
		// Act
		var fileLength = new FileLength(1000L);

		// Assert
		Assert.Equal(1000UL, fileLength.Value);
	}

	[Fact]
	public void NewNegativeLength_Should_Throw()
	{
		// Act + Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => new FileLength(-1L));
	}
}
