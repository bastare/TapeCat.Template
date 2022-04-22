namespace TapeCat.Template.Api;

using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public static class Program
{
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
			.ConfigureAppConfiguration ( ( hostBuilderContext , config ) =>
			  {
				  config
					   .AddJsonFile (
							 path: "./appsettings.json" ,
							 optional: false ,
							 reloadOnChange: true )

					   .AddJsonFile (
							 path: $"./appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json" ,
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