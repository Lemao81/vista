using Common.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Common.Presentation.Extensions;

public static class HttpContextExtensions
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
