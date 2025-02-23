using System.Diagnostics.CodeAnalysis;
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using static Amazon.Lambda.Annotations.APIGateway.HttpResults;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FileTransfer;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class AwsFileTransferFunctions
{
	public AwsFileTransferFunctions()
	{
	}

	[LambdaFunction(Policies = Policies.LambdaExecution)]
	[HttpApi(LambdaHttpMethod.Get, RouteBases.Pictures)]
	public async Task<IHttpResult> PingAsync(ILambdaContext context)
	{
		Console.WriteLine("PingAsync executed");
		await Task.Delay(100);

		return Ok("Pong");
	}
}
