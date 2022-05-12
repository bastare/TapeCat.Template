namespace TapeCat.Template.Api.Configurations.StartupConfiguration;

using Fluentx;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseServices (
		this IApplicationBuilder applicationBuilder ,
		IWebHostEnvironment webHostEnvironment ,
		IConfiguration configuration ,
		Dictionary<string , Action<IWebHostEnvironment , IConfiguration , IApplicationBuilder>> environmentConfigurations ,
		Action<IWebHostEnvironment , IConfiguration , IApplicationBuilder> generalConfiguration )
	{
		CurrentEnvironmentConfigurationIsSupported ( webHostEnvironment , environmentConfigurations );

		environmentConfigurations
			.Single ( environmentConfiguration => webHostEnvironment.IsEnvironment ( environmentConfiguration.Key ) )
			.Tap ( environmentConfiguration =>
			  {
				  var (_, injectService) = environmentConfiguration;

				  injectService.Invoke ( webHostEnvironment , configuration , applicationBuilder );
			  } );

		generalConfiguration.Invoke ( webHostEnvironment , configuration , applicationBuilder );

		return applicationBuilder;

		static void CurrentEnvironmentConfigurationIsSupported (
			IWebHostEnvironment webHostEnvironment ,
			Dictionary<string , Action<IWebHostEnvironment , IConfiguration , IApplicationBuilder>> environmentConfigurations )
		{
			environmentConfigurations
				.Any ( environmentConfiguration => environmentConfiguration.Key == webHostEnvironment.EnvironmentName )
				.IfFalse ( () => throw new ArgumentException ( $"Unsupported environment: {webHostEnvironment.EnvironmentName}" ) );
		}
	}
}
