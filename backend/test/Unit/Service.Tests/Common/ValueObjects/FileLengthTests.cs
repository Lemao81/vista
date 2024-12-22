using Domain.ValueObjects;

namespace Service.Tests.Common.ValueObjects;

public class FileLengthTests
{
	[Fact]
	public void When_new_given_valid_length_should_set_value()
	{
		// Act
		var fileLength = new FileLength(1000L);

		// Assert
		Assert.Equal(1000UL, fileLength.Value);
	}

	[Fact]
	public void When_new_given_negative_length_should_throw()
	{
		// Act + Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => new FileLength(-1L));
	}
}
