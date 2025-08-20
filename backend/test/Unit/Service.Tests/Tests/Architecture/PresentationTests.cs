using FluentValidation;
using NetArchTest.Rules;
using Service.Tests.Abstractions;

namespace Service.Tests.Tests.Architecture;

public class PresentationTests : ArchitectureTestBase
{
	public PresentationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Validators_Should_EndWithValidator()
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
