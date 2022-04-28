namespace TapeCat.Template.Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;

using GlobalExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class GlobalExceptionHandlerConfigurator
{
	public static void ExceptionFiltersConfigurator ( IApplicationBuilder applicationBuilder )
	{
		applicationBuilder.Run ( async httpContext =>
		  {
			  await ResolveGlobalExceptionHandler ( httpContext )
			  	.FormErrorResponseAsync ( httpContext );

			  await httpContext.Response.CompleteAsync ();

			  static ExceptionHandlerManager ResolveGlobalExceptionHandler ( HttpContext httpContext )
			  	=> httpContext.RequestServices.GetRequiredService<ExceptionHandlerManager> ();
		  } );
	}
}