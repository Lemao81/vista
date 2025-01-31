using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Presentation.Extensions;

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
