namespace TapeCat.Template.Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;

using GlobalExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class GlobalExceptionHandlerConfigurator
{
    public static void ExceptionFiltersConfigurator(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Run(
            httpContext =>
                ExceptionFiltersConfigurator(
                    httpContext,
                    exception: httpContext.ResolveException()));
    }

    public static async Task ExceptionFiltersConfigurator(HttpContext? httpContext, Exception? exception)
    {
        await ResolveGlobalExceptionHandler(httpContext!)
            .FormErrorResponseAsync(httpContext, exception);

        await httpContext!.Response.CompleteAsync();

        static ExceptionHandlerManager ResolveGlobalExceptionHandler(HttpContext httpContext)
            => httpContext.RequestServices.GetRequiredService<ExceptionHandlerManager>();
    }
}