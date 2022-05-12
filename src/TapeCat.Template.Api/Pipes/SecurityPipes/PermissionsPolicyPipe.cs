namespace TapeCat.Template.Api.Pipes.SecurityPipes;

using Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;

public static class PermissionsPolicyPipe
{
	public static IApplicationBuilder UsePermissionsPolicy ( this IApplicationBuilder applicationBuilder )
	{
		return applicationBuilder.Use ( async ( httpContext , next ) =>
		  {
			  httpContext.Response.Headers[ Headers.PermissionsPolicyHeaderName ] = BuildPermissionsPolicyBody ( in httpContext );

			  await next.Invoke ();
		  } );

		static string BuildPermissionsPolicyBody ( in HttpContext httpContext )
		{
			var siteUrl = new Uri ( $"{httpContext.Request.Scheme}://{httpContext.Request.Host}" );

			var stringBuilder =
				new StringBuilder ()
					.AppendJoin (
						separator: ", " ,
						values: new[]
						{
							$"fullscreen=(self {siteUrl} https://script.hotjar.com https://static.hotjar.com)" ,
							$"geolocation=(self {siteUrl})" ,
							$"payment=(self {siteUrl})" ,
							"camera=()" ,
							"microphone=()" ,
							"usb=()"
						} );

			return stringBuilder.ToString ();
		}
	}
}