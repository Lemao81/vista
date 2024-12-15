using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileTransfer;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true);

        var configuration = builder.Build();
        services.AddSingleton<IConfiguration>(configuration);
    }
}
