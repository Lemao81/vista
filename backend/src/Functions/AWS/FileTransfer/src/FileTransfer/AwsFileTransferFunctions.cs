using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using FileTransfer.Constants;
using static Amazon.Lambda.Annotations.APIGateway.HttpResults;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FileTransfer;

public class AwsFileTransferFunctions
{
	[LambdaFunction(Policies = Policies.LambdaExecution)]
	[HttpApi(LambdaHttpMethod.Get, RouteBases.Pictures)]
#pragma warning disable CA1822
	public async Task<IHttpResult> PingAsync(ILambdaContext context)
#pragma warning restore CA1822
	{
		Console.WriteLine("PingAsync executed");
		await Task.Delay(100);

		return Ok("Pong");
	}
}
