using NetArchTest.Rules;
using SharedKernel;
using Xunit.Abstractions;

namespace Service.Tests.Architecture;

public class DomainTests : ArchitectureTestBase
{
	public DomainTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Entities_should_be_sealed()
	{
		// Act
		var result = Types.InAssemblies(DomainAssemblies).That().Inherit(typeof(Entity<>)).Should().BeSealed().GetResult();
		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void DomainEvents_should_end_name_with_DomainEvent()
	{
		// Act
		var result = Types.InAssemblies(DomainAssemblies)
			.That()
			.ImplementInterface(typeof(IDomainEvent))
			.Should()
			.HaveNameEndingWith("DomainEvent", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Errors_should_end_name_with_Error()
	{
		// Act
		var result = Types.InAssemblies(DomainAssemblies)
			.That()
			.Inherit(typeof(Error))
			.Should()
			.HaveNameEndingWith("Error", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Domain_should_not_depend_on_outer_layers()
	{
		// Arrange
		var outerAssemblies = ApplicationAssemblies.Concat(PersistenceAssemblies)
			.Concat(InfrastructureAssemblies)
			.Concat(WebApiAssemblies)
			.GroupBy(a => a.GetName().Name?.Split(".")[0]!)
			.ToDictionary(g => g.Key, g => g.ToArray());

		foreach (var domainAssembly in DomainAssemblies)
		{
			var key             = domainAssembly.GetName().Name?.Split(".")[0];
			var outerNamespaces = outerAssemblies[key!].Select(a => a.GetName().Name?.Split(".")[1]).ToArray();

			// Act
			var result = Types.InAssembly(domainAssembly).Should().NotHaveDependencyOnAny(outerNamespaces).GetResult();

			PrintFailingTypes(result);

			// Assert
			Assert.True(result.IsSuccessful);
		}
	}
}
