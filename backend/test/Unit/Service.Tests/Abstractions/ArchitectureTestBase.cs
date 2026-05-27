using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace Service.Tests.Abstractions;

public abstract class ArchitectureTestBase
{
	protected ArchitectureTestBase(ITestOutputHelper testOutputHelper)
	{
		TestOutputHelper = testOutputHelper;
	}

	protected ITestOutputHelper TestOutputHelper { get; }

	protected static readonly Assembly[] DomainAssemblies =
	[
		typeof(FileTransfer.Domain.IDomainAssemblyMarker).Assembly, typeof(Authentication.Domain.IDomainAssemblyMarker).Assembly,
	];

	protected static readonly Assembly[] ApplicationAssemblies =
	[
		typeof(FileTransfer.Application.IApplicationAssemblyMarker).Assembly, typeof(Authentication.Application.IApplicationAssemblyMarker).Assembly,
	];

	protected static readonly Assembly[] InfrastructureAssemblies =
	[
		typeof(FileTransfer.Infrastructure.IInfrastructureAssemblyMarker).Assembly,
		typeof(Authentication.Infrastructure.IInfrastructureAssemblyMarker).Assembly,
	];

	protected static readonly Assembly[] PresentationAssemblies =
	[
		typeof(FileTransfer.Presentation.IPresentationAssemblyMarker).Assembly, typeof(Authentication.Presentation.IPresentationAssemblyMarker).Assembly,
	];

	protected static readonly Assembly[] WebApiAssemblies =
	[
		typeof(FileTransfer.WebApi.IWebApiAssemblyMarker).Assembly, typeof(Authentication.WebApi.IWebApiAssemblyMarker).Assembly,
	];

	protected static IObjectProvider<IType> DomainLayer { get; } = Types()
		.That()
		.ResideInAssembly(DomainAssemblies[0], DomainAssemblies[1..])
		.As(nameof(DomainLayer));

	protected IObjectProvider<IType> ApplicationLayer { get; } = Types()
		.That()
		.ResideInAssembly(ApplicationAssemblies[0], ApplicationAssemblies[1..])
		.As(nameof(ApplicationLayer));

	protected IObjectProvider<IType> InfrastructureLayer { get; } = Types()
		.That()
		.ResideInAssembly(InfrastructureAssemblies[0], InfrastructureAssemblies[1..])
		.As(nameof(InfrastructureLayer));

	protected IObjectProvider<IType> PresentationLayer { get; } = Types()
		.That()
		.ResideInAssembly(PresentationAssemblies[0], PresentationAssemblies[1..])
		.As(nameof(PresentationLayer));

	protected IObjectProvider<IType> WebApiLayer { get; } = Types().That().ResideInAssembly(WebApiAssemblies[0], WebApiAssemblies[1..]).As(nameof(WebApiLayer));

	protected static Architecture Architecture =>
		new ArchLoader().LoadAssembliesIncludingDependencies(
				[..DomainAssemblies, ..ApplicationAssemblies, ..InfrastructureAssemblies, ..PresentationAssemblies, ..WebApiAssemblies,])
			.Build();
}
