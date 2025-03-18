using System.Reflection;
using FileTransfer.Application;
using FileTransfer.Domain;
using FileTransfer.Infrastructure;
using FileTransfer.Persistence;
using FileTransfer.Presentation;
using FileTransfer.WebApi;
using NetArchTest.Rules;
using Lemao.UtilExtensions;
using Xunit.Abstractions;

namespace Service.Tests.Architecture;

public abstract class ArchitectureTestBase
{
	protected ArchitectureTestBase(ITestOutputHelper testOutputHelper)
	{
		TestOutputHelper = testOutputHelper;
	}

	protected ITestOutputHelper     TestOutputHelper         { get; }
	protected IEnumerable<Assembly> DomainAssemblies         { get; } = [typeof(DomainAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> ApplicationAssemblies    { get; } = [typeof(ApplicationAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> InfrastructureAssemblies { get; } = [typeof(InfrastructureAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> PersistenceAssemblies    { get; } = [typeof(PersistenceAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> PresentationAssemblies   { get; } = [typeof(PresentationAssemblyMarker).Assembly];
	protected IEnumerable<Assembly> WebApiAssemblies         { get; } = [typeof(WebApiAssemblyMarker).Assembly];

	protected void PrintFailingTypes(TestResult result) => TestOutputHelper.WriteLine(result.FailingTypeNames.ToCommaSeparated());
}
