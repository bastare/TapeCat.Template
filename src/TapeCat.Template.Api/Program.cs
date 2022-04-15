namespace TapeCat.Template.Api;

using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class Program
{
	private const string GeneralAppSettingFilePath = "./appsettings.json";

	private const string EnvironmentsAppSettingFilePath = "./appsettings.{env.EnvironmentName}.json";

	public static void Main ( string[] args )
	{
		CreateHostBuilder ( args )
			.Build ()
			.Run ();
	}

	public static IHostBuilder CreateHostBuilder ( string[] args )
		=> Host
			.CreateDefaultBuilder ( args )
			.UseServiceProviderFactory ( new AutofacServiceProviderFactory () )
			.ConfigureAppConfiguration ( ( _ , config ) =>
			  {
				  config
					  .AddJsonFile (
							path: GeneralAppSettingFilePath ,
							optional: false ,
							reloadOnChange: true )

					  .AddJsonFile (
							path: EnvironmentsAppSettingFilePath ,
							optional: true ,
							reloadOnChange: true )

					  .AddEnvironmentVariables ();
			  } )
			.ConfigureWebHostDefaults ( webBuilder =>
			  {
				  webBuilder
					.UseIISIntegration ()
					.UseStartup<Startup> ();
			  } );
}