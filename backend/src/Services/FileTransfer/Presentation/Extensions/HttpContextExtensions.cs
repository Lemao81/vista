using Common.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace FileTransfer.Presentation.Extensions;

internal static class HttpContextExtensions
{
	public static void MaybeAddError(this HttpContext httpContext, Result result)
	{
		if (!result.IsFailure)
		{
			return;
		}

		httpContext.Items.Add(HttpContextItemKeys.ErrorCode, result.Error.Code);
	}
}
