namespace TapeCat.Template.Api.Configurations.StartupConfiguration;

using Common.Extensions;
using Infostructure.CrossCutting.Configurators.SwaggerConfigurators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pipes;
using Pipes.SecurityPipes;

public static class ApplicationBuilderExtensions
{
	public static void UseServicesOn ( this IApplicationBuilder applicationBuilder , IWebHostEnvironment webHostEnvironment )
	{
		if ( webHostEnvironment.IsProduction () || webHostEnvironment.IsStage () )
			UseServicesOnProduction ( applicationBuilder , webHostEnvironment );
		else if ( webHostEnvironment.IsDevelopment () )
			UseServicesOnDevelopment ( applicationBuilder , webHostEnvironment );
	}

	private static void UseServicesOnProduction ( IApplicationBuilder applicationBuilder , IWebHostEnvironment webHostEnvironment )
	{
		applicationBuilder.UseSecurityHeaders ( webHostEnvironment );
	}

	private static void UseServicesOnDevelopment ( IApplicationBuilder applicationBuilder , IWebHostEnvironment _ )
	{
		applicationBuilder.UseCors ( builder =>
		  {
			  builder
				  .AllowAnyOrigin ()
				  .AllowAnyHeader ()
				  .AllowAnyMethod ();
		  } );

		applicationBuilder.UseAccessControlExposeHeaders ();

		applicationBuilder.UseDeveloperExceptionPage ();

		applicationBuilder.UseSwagger ();

		applicationBuilder.UseSwaggerUI ( SwaggerConfigurator.SwaggerUIConfigurator );
	}
}
