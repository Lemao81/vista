using ArchUnitNET.xUnitV3;
using Service.Tests.Abstractions;
using SharedKernel;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Service.Tests.Tests.Architecture;

public class DomainTests : ArchitectureTestBase
{
	public DomainTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Entities_Should_BeSealed()
	{
		Classes().That().AreAssignableTo(typeof(Entity<>)).And().AreNotAbstract().Should().BeSealed().Check(Architecture);
	}

	[Fact]
	public void DomainEvents_Should_EndWithDomainEvent()
	{
		Classes().That().ImplementInterface(typeof(IDomainEvent)).Should().HaveNameEndingWith("DomainEvent").Check(Architecture);
	}

	[Fact]
	public void Errors_Should_EndWithError()
	{
		Classes().That().AreAssignableTo(typeof(Error)).Should().HaveNameEndingWith("Error").Check(Architecture);
	}

	[Fact]
	public void Errors_Should_BeRecords()
	{
		Classes().That().AreAssignableTo(typeof(Error)).Should().BeRecord().Check(Architecture);
	}

	[Fact]
	public void Domain_Should_NotHaveDependencyOnApplication()
	{
		Types().That().Are(DomainLayer).Should().NotDependOnAny(ApplicationLayer).Check(Architecture);
	}

	[Fact]
	public void Domain_Should_NotHaveDependencyOnInfrastructure()
	{
		Types().That().Are(DomainLayer).Should().NotDependOnAny(InfrastructureLayer).Check(Architecture);
	}

	[Fact]
	public void Domain_Should_NotHaveDependencyOnPresentation()
	{
		Types().That().Are(DomainLayer).Should().NotDependOnAny(PresentationLayer).Check(Architecture);
	}

	[Fact]
	public void Domain_Should_NotHaveDependencyOnWebApi()
	{
		Types().That().Are(DomainLayer).Should().NotDependOnAny(WebApiLayer).Check(Architecture);
	}
}
