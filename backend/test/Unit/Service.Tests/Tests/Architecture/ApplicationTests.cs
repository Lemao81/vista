using ArchUnitNET.xUnitV3;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Command;
using FluentValidation;
using MediatR;
using Service.Tests.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Service.Tests.Tests.Architecture;

public class ApplicationTests : ArchitectureTestBase
{
	public ApplicationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Commands_Should_EndWithCommand()
	{
		Classes().That().ImplementInterface(typeof(IBaseCommand)).Should().HaveNameEndingWith("Command").Check(Architecture);
	}

	[Fact]
	public void CommandHandlers_Should_EndWithCommandHandler()
	{
		Classes().That().ImplementInterface(typeof(ICommandHandler<,>)).Should().HaveNameEndingWith("CommandHandler").Check(Architecture);
	}

	[Fact]
	public void Validators_Should_EndWithValidator()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(IValidator))
			.And()
			.DoNotResideInNamespaceMatching("FluentValidation")
			.Should()
			.HaveNameEndingWith("Validator")
			.Check(Architecture);
	}

	[Fact]
	public void DomainEventHandlers_Should_EndWithDomainEventHandler()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(INotificationHandler<>))
			.And()
			.DoNotResideInNamespaceMatching("MediatR")
			.Should()
			.HaveNameEndingWith("DomainEventHandler")
			.Check(Architecture);
	}

	[Fact]
	public void PipelineBehaviors_Should_EndWithPipelineBehavior()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(IPipelineBehavior<,>))
			.And()
			.AreNotAbstract()
			.And()
			.DoNotResideInNamespaceMatching("MediatR")
			.Should()
			.HaveNameEndingWith("PipelineBehavior`2")
			.Check(Architecture);
	}

	[Fact]
	public void RequestHandlers_Should_BeInternalSealed()
	{
		Classes().That().ImplementInterface(typeof(IRequestHandler<,>)).Should().BeInternal().AndShould().BeSealed().Check(Architecture);
	}

	[Fact]
	public void NotificationHandlers_Should_BeInternalSealed()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(INotificationHandler<>))
			.And()
			.DoNotResideInNamespaceMatching("MediatR")
			.Should()
			.BeInternal()
			.AndShould()
			.BeSealed()
			.Check(Architecture);
	}

	[Fact(Skip = "throws NullReferenceException in ArchUnitNET.Domain.Extensions.NamingExtensions.FullNameEquals()")]
	public void Application_Should_NotHaveDependencyOnPostgres()
	{
		Types().That().Are(ApplicationLayer).Should().NotDependOnAnyTypesThat().ResideInNamespace("Npgsql").Check(Architecture);
	}

	[Fact]
	public void Application_Should_NotHaveDependencyOnInfrastructure()
	{
		Types().That().Are(ApplicationLayer).Should().NotDependOnAny(InfrastructureLayer).Check(Architecture);
	}

	[Fact]
	public void Application_Should_NotHaveDependencyOnPresentation()
	{
		Types().That().Are(ApplicationLayer).Should().NotDependOnAny(PresentationLayer).Check(Architecture);
	}

	[Fact]
	public void Application_Should_NotHaveDependencyOnWebApi()
	{
		Types().That().Are(ApplicationLayer).Should().NotDependOnAny(WebApiLayer).Check(Architecture);
	}
}
