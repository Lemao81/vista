using Service.Tests.Utilities;
using SharedKernel;

namespace Service.Tests.Tests.Common;

public class BasePipelineBehaviorTests
{
	[Fact]
	public void ErrorNonGeneric_Should_ReturnFailureResult()
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
	public void ErrorGeneric_Should_ReturnFailureResult()
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
