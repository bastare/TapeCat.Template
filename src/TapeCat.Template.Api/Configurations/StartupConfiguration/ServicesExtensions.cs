namespace TapeCat.Template.Api.Configurations.StartupConfiguration;

using Fluentx;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ServicesExtensions
{
	public static IServiceCollection AddDependency (
		this IServiceCollection serviceCollection ,
		IWebHostEnvironment webHostEnvironment ,
		IConfiguration configuration ,
		Dictionary<string , Action<IWebHostEnvironment , IConfiguration , IServiceCollection>> environmentConfigurations )
	{
		CurrentEnvironmentConfigurationIsSupported ( webHostEnvironment , environmentConfigurations );

		environmentConfigurations
			.Single ( environmentConfiguration => webHostEnvironment.IsEnvironment ( environmentConfiguration.Key ) )
			.Tap ( environmentConfiguration =>
			  {
				  var (_, injectService) = environmentConfiguration;

				  injectService.Invoke ( webHostEnvironment , configuration , serviceCollection );
			  } );

		return serviceCollection;

		static void CurrentEnvironmentConfigurationIsSupported (
			IWebHostEnvironment webHostEnvironment ,
			IDictionary<string , Action<IWebHostEnvironment , IConfiguration , IServiceCollection>> environmentConfigurations )
		{
			environmentConfigurations
				.Any ( environmentConfiguration => environmentConfiguration.Key == webHostEnvironment.EnvironmentName )
				.IfFalse ( () => throw new ArgumentException ( $"Unsupported environment: {webHostEnvironment.EnvironmentName}" ) );
		}
	}
}