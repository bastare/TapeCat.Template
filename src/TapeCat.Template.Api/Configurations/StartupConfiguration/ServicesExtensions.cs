namespace TapeCat.Template.Api.Configurations.StartupConfiguration;

using Common.Extensions;
using Infrastructure.CrossCutting.Configurators.SwaggerConfigurators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ServicesExtensions
{
	public static IServiceCollection InjectDependency ( this IServiceCollection serviceCollection ,
														IWebHostEnvironment webHostEnvironment ,
														IConfiguration configuration )
	{
		InjectDependencyOn ( serviceCollection , webHostEnvironment , configuration );

		return serviceCollection;
	}

	private static void InjectDependencyOn ( IServiceCollection serviceCollection ,
											 IWebHostEnvironment webHostEnvironment ,
											 IConfiguration configuration )
	{
		if ( webHostEnvironment.IsProduction () || webHostEnvironment.IsStage () )
			AddServicesOnProduction ( serviceCollection , webHostEnvironment , configuration );
		else if ( webHostEnvironment.IsDevelopment () )
			AddServicesOnDevelopment ( serviceCollection , webHostEnvironment , configuration );
	}

	private static void AddServicesOnProduction ( IServiceCollection _ , IWebHostEnvironment __ , IConfiguration ___ ) { }

	private static void AddServicesOnDevelopment ( IServiceCollection serviceCollection , IWebHostEnvironment _ , IConfiguration __ )
	{
		serviceCollection.AddSwaggerGen ( SwaggerConfigurator.SwaggerDIConfigurator );

		serviceCollection.AddSwaggerGenNewtonsoftSupport ();
	}
}