using System.Reflection;
using Lemao.UtilExtensions;
using TestResult = NetArchTest.Rules.TestResult;

namespace Service.Tests.Abstractions;

public abstract class ArchitectureTestBase
{
	protected ArchitectureTestBase(ITestOutputHelper testOutputHelper)
	{
		TestOutputHelper = testOutputHelper;
	}

	protected ITestOutputHelper TestOutputHelper { get; }

	protected IEnumerable<Assembly> DomainAssemblies { get; } =
	[
		typeof(FileTransfer.Domain.IDomainAssemblyMarker).Assembly, typeof(Authentication.Domain.IDomainAssemblyMarker).Assembly,
	];

	protected IEnumerable<Assembly> ApplicationAssemblies { get; } =
	[
		typeof(FileTransfer.Application.IApplicationAssemblyMarker).Assembly, typeof(Authentication.Application.IApplicationAssemblyMarker).Assembly,
	];

	protected IEnumerable<Assembly> InfrastructureAssemblies { get; } =
	[
		typeof(FileTransfer.Infrastructure.IInfrastructureAssemblyMarker).Assembly,
		typeof(Authentication.Infrastructure.IInfrastructureAssemblyMarker).Assembly,
	];

	protected IEnumerable<Assembly> PresentationAssemblies { get; } =
	[
		typeof(FileTransfer.Presentation.IPresentationAssemblyMarker).Assembly, typeof(Authentication.Presentation.IPresentationAssemblyMarker).Assembly,
	];

	protected IEnumerable<Assembly> WebApiAssemblies { get; } =
	[
		typeof(FileTransfer.WebApi.IWebApiAssemblyMarker).Assembly, typeof(Authentication.WebApi.IWebApiAssemblyMarker).Assembly,
	];

	protected void PrintFailingTypes(TestResult result) => TestOutputHelper.WriteLine(result.FailingTypeNames.ToCommaSeparated());
}
