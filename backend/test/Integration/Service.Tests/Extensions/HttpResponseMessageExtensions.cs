using Xunit.Abstractions;

namespace Service.Tests.Extensions;

public static class HttpResponseMessageExtensions
{
	public static async Task PrintContentAsync(this HttpResponseMessage response, ITestOutputHelper outputHelper) =>
		outputHelper.WriteLine(await response.Content.ReadAsStringAsync());
}
