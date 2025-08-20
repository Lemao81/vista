using NetArchTest.Rules;
using Service.Tests.Abstractions;
using SharedKernel;

namespace Service.Tests.Tests.Architecture;

public class DomainTests : ArchitectureTestBase
{
	public DomainTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Entities_Should_BeSealed()
	{
		// Act
		var result = Types.InAssemblies(DomainAssemblies).That().Inherit(typeof(Entity<>)).Should().BeSealed().GetResult();
		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void DomainEvents_Should_EndWithDomainEvent()
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
	public void Errors_Should_EndWithError()
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
	public void Domain_Should_NotDependOnOuterLayers()
	{
		// Arrange
		var outerAssemblies = ApplicationAssemblies.Concat(InfrastructureAssemblies)
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
