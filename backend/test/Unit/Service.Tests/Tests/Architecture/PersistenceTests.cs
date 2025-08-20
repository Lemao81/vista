using NetArchTest.Rules;
using Service.Tests.Abstractions;
using SharedKernel;

namespace Service.Tests.Tests.Architecture;

public class PersistenceTests : ArchitectureTestBase
{
	public PersistenceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Repositories_Should_EndWithRepository()
	{
		// Act
		var result = Types.InAssemblies(InfrastructureAssemblies)
			.That()
			.ImplementInterface(typeof(IRepository))
			.Should()
			.HaveNameEndingWith("Repository", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Repositories_Should_BeInternalSealed()
	{
		// Act
		var result = Types.InAssemblies(InfrastructureAssemblies)
			.That()
			.ImplementInterface(typeof(IRepository))
			.Should()
			.NotBePublic()
			.And()
			.BeSealed()
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}
}
