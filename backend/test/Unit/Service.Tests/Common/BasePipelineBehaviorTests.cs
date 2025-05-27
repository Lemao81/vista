using Service.Tests.Utilities;
using SharedKernel;

namespace Service.Tests.Common;

public class BasePipelineBehaviorTests
{
	[Fact]
	public void CreateErrorResultT0_Given_error_Should_return_failure_result()
	{
		// Arrange
		var error = Errors.EntityNotFound;

		// Act
		var response = NonGenericTestBasePipelineBehavior.CreateErrorResultPublic(error);

		// Assert
		Assert.True(response.IsFailure);
		Assert.Equal(error, response.Error);
	}

	[Fact]
	public void CreateErrorResultT1_Given_error_Should_return_failure_result()
	{
		// Arrange
		var error = Errors.EntityNotFound;

		// Act
		var response = GenericTestBasePipelineBehavior.CreateErrorResultPublic(error);

		// Assert
		Assert.True(response.IsFailure);
		Assert.Equal(error, response.Error);
	}
}
