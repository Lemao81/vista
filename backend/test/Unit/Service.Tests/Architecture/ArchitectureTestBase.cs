using System.Reflection;
using FileTransfer.Application;
using FileTransfer.Domain;
using FileTransfer.Infrastructure;
using FileTransfer.Presentation;
using FileTransfer.WebApi;
using Lemao.UtilExtensions;
using TestResult = NetArchTest.Rules.TestResult;

namespace Service.Tests.Architecture;

public abstract class ArchitectureTestBase
{
	protected ArchitectureTestBase(ITestOutputHelper testOutputHelper)
	{
		TestOutputHelper = testOutputHelper;
	}

	protected ITestOutputHelper     TestOutputHelper         { get; }
	protected IEnumerable<Assembly> DomainAssemblies         { get; } = [typeof(IDomainAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> ApplicationAssemblies    { get; } = [typeof(IApplicationAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> InfrastructureAssemblies { get; } = [typeof(IInfrastructureAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> PresentationAssemblies   { get; } = [typeof(IPresentationAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> WebApiAssemblies         { get; } = [typeof(IWebApiAssemblyMarker).Assembly];

	protected void PrintFailingTypes(TestResult result) => TestOutputHelper.WriteLine(result.FailingTypeNames.ToCommaSeparated());
}
