using Service.Tests.Abstractions;
using SharedKernel;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Service.Tests.Tests.Architecture;

public class PersistenceTests : ArchitectureTestBase
{
	public PersistenceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Repositories_Should_EndWithRepository()
	{
		Classes().That().ImplementInterface(typeof(IRepository)).Should().HaveNameEndingWith("Repository");
	}

	[Fact]
	public void Repositories_Should_BeInternalSealed()
	{
		Classes().That().ImplementInterface(typeof(IRepository)).Should().BeInternal().AndShould().BeSealed();
	}
}
