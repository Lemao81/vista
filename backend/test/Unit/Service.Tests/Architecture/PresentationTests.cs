using FluentValidation;
using NetArchTest.Rules;
using Xunit.Abstractions;

namespace Service.Tests.Architecture;

public class PresentationTests : ArchitectureTestBase
{
	public PresentationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Validators_should_end_name_with_Validator()
	{
		// Act
		var result = Types.InAssemblies(PresentationAssemblies)
			.That()
			.ImplementInterface(typeof(IValidator))
			.Should()
			.HaveNameEndingWith("Validator", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}
}
