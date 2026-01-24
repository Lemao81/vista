using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.Tests.Utilities;

public static class TestHelper
{
	public static HttpClient CreateAuthorizedHttpClient<T>(WebApplicationFactory<T> webApplicationFactory)
		where T : class
	{
		var httpClient = webApplicationFactory.CreateClient();
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, TestConstants.Jwt);

		return httpClient;
	}

	public static async Task AwaitHealthiness<T>(WebApplicationFactory<T> webApplicationFactory)
		where T : class
	{
		var healthCheckService = webApplicationFactory.Services.GetRequiredService<HealthCheckService>();
		var timeoutTask        = Task.Delay(TimeSpan.FromSeconds(10));

		while (true)
		{
			var checkTask = healthCheckService.CheckHealthAsync();
			var task      = await Task.WhenAny(timeoutTask, checkTask);
			if (task == timeoutTask)
			{
				throw new TimeoutException();
			}

			if ((await checkTask).Status == HealthStatus.Healthy)
			{
				break;
			}

			await Task.Delay(TimeSpan.FromMilliseconds(200));
		}
	}
}
