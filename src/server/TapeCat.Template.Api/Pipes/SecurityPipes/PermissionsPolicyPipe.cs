namespace TapeCat.Template.Api.Pipes.SecurityPipes;

using Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

public static class PermissionsPolicyPipe
{
    public static IApplicationBuilder UsePermissionsPolicy(this IApplicationBuilder applicationBuilder, Func<Uri, IEnumerable<string>> configuration)
    {
        return applicationBuilder.Use(async (httpContext, next) =>
          {
              httpContext.Response.Headers[Headers.PermissionsPolicyHeaderName] = BuildPermissionsPolicyBody(httpContext, configuration);

              await next.Invoke();
          });

        static string BuildPermissionsPolicyBody(HttpContext httpContext, Func<Uri, IEnumerable<string>> configuration)
            => string.Join(
                separator: ", ",
                configuration(new($"{httpContext.Request.Scheme}://{httpContext.Request.Host}")));
    }
}