namespace TapeCat.Template.Api.Pipes;

using Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

public static class AccessControlExposePipe
{
	public static void UseAccessControlExposeHeaders ( this IApplicationBuilder applicationBuilder )
	{
		applicationBuilder.Use ( async ( httpContext , next ) =>
		  {
			  httpContext.Response.Headers[ HeaderNames.AccessControlExposeHeaders ] = Headers.CustomHeaders.PaginationHeaderName;

			  await next.Invoke ();
		  } );
	}
}