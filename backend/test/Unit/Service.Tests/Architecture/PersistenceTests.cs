using Domain.Abstractions;
using NetArchTest.Rules;
using Xunit.Abstractions;

namespace Service.Tests.Architecture;

public class PersistenceTests : BaseArchitectureTest
{
	public PersistenceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Repositories_should_end_name_with_Repository()
	{
		// Act
		var result = Types.InAssemblies(PersistenceAssemblies)
			.That()
			.ImplementInterface(typeof(IRepository))
			.Should()
			.HaveNameEndingWith("Repository")
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Repositories_should_be_internal_sealed()
	{
		// Act
		var result = Types.InAssemblies(PersistenceAssemblies)
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
