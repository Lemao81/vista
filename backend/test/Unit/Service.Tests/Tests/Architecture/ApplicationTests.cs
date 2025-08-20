using Common.Application.Abstractions;
using Common.Application.Abstractions.Command;
using FluentValidation;
using MediatR;
using NetArchTest.Rules;
using Service.Tests.Abstractions;

namespace Service.Tests.Tests.Architecture;

public class ApplicationTests : ArchitectureTestBase
{
	public ApplicationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Commands_Should_EndWithCommand()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(IBaseCommand))
			.Should()
			.HaveNameEndingWith("Command", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void CommandHandlers_Should_EndWithCommandHandler()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(ICommandHandler<,>))
			.Should()
			.HaveNameEndingWith("CommandHandler", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Validators_Should_EndWithValidator()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(IValidator))
			.Should()
			.HaveNameEndingWith("Validator", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void DomainEventHandlers_Should_EndWithDomainEventHandler()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(INotificationHandler<>))
			.Should()
			.HaveNameEndingWith("DomainEventHandler", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void PipelineBehaviors_Should_EndWithPipelineBehavior()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(IPipelineBehavior<,>))
			.Should()
			.HaveNameEndingWith("PipelineBehavior", StringComparison.Ordinal)
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void RequestHandlers_Should_BeInternalSealed()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(IRequestHandler<,>))
			.Should()
			.NotBePublic()
			.And()
			.BeSealed()
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void NotificationHandlers_Should_BeInternalSealed()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies)
			.That()
			.ImplementInterface(typeof(INotificationHandler<>))
			.Should()
			.NotBePublic()
			.And()
			.BeSealed()
			.GetResult();

		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Application_Should_NotHaveDependencyOnPostgres()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies).ShouldNot().HaveDependencyOn("Npgsql.EntityFrameworkCore.PostgreSQL").GetResult();
		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Application_Should_NotDependOnOuterLayers()
	{
		// Arrange
		var outerAssemblies = InfrastructureAssemblies.Concat(InfrastructureAssemblies)
			.Concat(WebApiAssemblies)
			.GroupBy(a => a.GetName().Name?.Split(".")[0]!)
			.ToDictionary(g => g.Key, g => g.ToArray());

		foreach (var applicationAssembly in ApplicationAssemblies)
		{
			var key             = applicationAssembly.GetName().Name?.Split(".")[0];
			var outerNamespaces = outerAssemblies[key!].Select(a => a.GetName().Name?.Split(".")[1]).ToArray();

			// Act
			var result = Types.InAssembly(applicationAssembly).Should().NotHaveDependencyOnAny(outerNamespaces).GetResult();

			PrintFailingTypes(result);

			// Assert
			Assert.True(result.IsSuccessful);
		}
	}
}
