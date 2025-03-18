using Service.Tests.Utilities;
using SharedKernel;

namespace Service.Tests.Common;

public class BasePipelineBehaviorTests
{
	private readonly NonGenericTestBasePipelineBehavior _classUnderTestT0;
	private readonly GenericTestBasePipelineBehavior    _classUnderTestT1;

	public BasePipelineBehaviorTests()
	{
		_classUnderTestT0 = new NonGenericTestBasePipelineBehavior();
		_classUnderTestT1 = new GenericTestBasePipelineBehavior();
	}

	[Fact]
	public void CreateErrorResultT0_Given_error_Should_return_failure_result()
	{
		// Arrange
		var error = Errors.EntityNotFound;

		// Act
		var response = _classUnderTestT0.CreateErrorResultPublic(error);

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
		var response = _classUnderTestT1.CreateErrorResultPublic(error);

		// Assert
		Assert.True(response.IsFailure);
		Assert.Equal(error, response.Error);
	}
}
