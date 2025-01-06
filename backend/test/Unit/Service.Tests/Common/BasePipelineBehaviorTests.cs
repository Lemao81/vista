using Domain;
using Service.Tests.Utilities;

namespace Service.Tests.Common;

public class BasePipelineBehaviorTests
{
	private readonly TestBasePipelineBehaviorT0 _classUnderTestT0;
	private readonly TestBasePipelineBehaviorT1 _classUnderTestT1;

	public BasePipelineBehaviorTests()
	{
		_classUnderTestT0 = new TestBasePipelineBehaviorT0();
		_classUnderTestT1 = new TestBasePipelineBehaviorT1();
	}

	[Fact]
	public void When_CreateErrorResult_given_error_should_return_failure_result()
	{
		// Arrange
		var error = Errors.EntityNotFound;

		// Act
		var responseT0 = _classUnderTestT0.CreateErrorResultPublic(error);
		var responseT1 = _classUnderTestT1.CreateErrorResultPublic(error);

		// Assert
		Assert.True(responseT0.IsFailure);
		Assert.Equal(error, responseT0.Error);
		Assert.True(responseT1.IsFailure);
		Assert.Equal(error, responseT1.Error);
	}
}
