using System.Runtime.CompilerServices;

namespace Service.Tests;

internal sealed class VerifyIntegrationSettings
{
	[ModuleInitializer]
	public static void Initialize()
	{
		VerifyHttp.Initialize();
		Recording.Start();
	}
}
