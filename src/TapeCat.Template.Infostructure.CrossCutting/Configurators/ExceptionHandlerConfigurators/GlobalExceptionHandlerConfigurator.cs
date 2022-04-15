namespace TapeCat.Template.Infostructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;

using GlobalExceptionHandler.Factories;
using Microsoft.AspNetCore.Builder;

public static class GlobalExceptionHandlerConfigurator
{
	public static void ExceptionFiltersConfigurator ( IApplicationBuilder applicationBuilder )
	{
		applicationBuilder.Run ( async httpContext =>
		  {
			  await GlobalExceptionHandlerFactory.Create ( httpContext )
				  .FormErrorResponseAsync ( httpContext.RequestAborted );

			  await httpContext.Response.CompleteAsync ();
		  } );
	}
}