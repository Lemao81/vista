using Common.Application.Abstractions;
using FluentValidation;
using MediatR;
using NetArchTest.Rules;
using Xunit.Abstractions;

namespace Service.Tests.Architecture;

public class ApplicationTests : ArchitectureTestBase
{
	public ApplicationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
	{
	}

	[Fact]
	public void Commands_should_end_name_with_Command()
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
	public void CommandHandlers_should_end_name_with_CommandHandler()
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
	public void Validators_should_end_name_with_Validator()
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
	public void DomainEventHandlers_should_end_name_with_DomainEventHandler()
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
	public void PipelineBehaviors_should_end_name_with_PipelineBehavior()
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
	public void RequestHandlers_should_be_internal_sealed()
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
	public void NotificationHandlers_should_be_internal_sealed()
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
	public void Application_should_not_have_dependency_on_postgres()
	{
		// Act
		var result = Types.InAssemblies(ApplicationAssemblies).ShouldNot().HaveDependencyOn("Npgsql.EntityFrameworkCore.PostgreSQL").GetResult();
		PrintFailingTypes(result);

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Application_should_not_depend_on_outer_layers()
	{
		// Arrange
		var outerAssemblies = PersistenceAssemblies.Concat(InfrastructureAssemblies)
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
