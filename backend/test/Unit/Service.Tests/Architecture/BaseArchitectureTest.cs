using System.Reflection;
using Application;
using Domain;
using Infrastructure;
using NetArchTest.Rules;
using Persistence;
using Lemao.UtilExtensions;
using WebApi;
using Xunit.Abstractions;

namespace Service.Tests.Architecture;

public abstract class BaseArchitectureTest
{
	protected BaseArchitectureTest(ITestOutputHelper testOutputHelper)
	{
		TestOutputHelper = testOutputHelper;
	}

	protected readonly ITestOutputHelper TestOutputHelper;
	protected readonly Assembly[]        DomainAssemblies         = [typeof(DomainAssemblyMarker).Assembly];
	protected readonly Assembly[]        ApplicationAssemblies    = [typeof(ApplicationAssemblyMarker).Assembly];
	protected readonly Assembly[]        InfrastructureAssemblies = [typeof(InfrastructureAssemblyMarker).Assembly];
	protected readonly Assembly[]        PersistenceAssemblies    = [typeof(PersistenceAssemblyMarker).Assembly];
	protected readonly Assembly[]        WebApiAssemblies         = [typeof(WebApiAssemblyMarker).Assembly];

	protected void PrintFailingTypes(TestResult result) => TestOutputHelper.WriteLine(result.FailingTypeNames.ToCommaSeparated());
}
