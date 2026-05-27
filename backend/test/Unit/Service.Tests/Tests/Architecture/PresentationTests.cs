using FluentValidation;
using Service.Tests.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Service.Tests.Tests.Architecture;

public class PresentationTests : ArchitectureTestBase
{
	public PresentationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Validators_Should_EndWithValidator()
	{
		Classes().That().ImplementInterface(typeof(IValidator)).Should().HaveNameEndingWith("Validator");
	}
}
