namespace Tests.Common;

#pragma warning disable CA1515
public static class VerifySettingsFactory
#pragma warning restore CA1515
{
	private static readonly Lazy<VerifySettings> ScrubGuidsSettingsLazy = new(() =>
	{
		var settings = new VerifySettings();
		settings.ScrubInlineGuids();
		settings.ScrubMembers("traceId", "requestId");

		return settings;
	});

	public static VerifySettings ScrubGuidsSettings => ScrubGuidsSettingsLazy.Value;
}
