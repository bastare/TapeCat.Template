using Autofac;
using Autofac.Extensions.DependencyInjection;
using TapeCat.Template.Api;

var builder = WebApplication.CreateBuilder (
	options: new ()
	{
		Args = args ,
		WebRootPath = "webroot"
	} );

var startup = new Startup ( builder.Configuration , builder.Environment );

builder.Host
	.UseServiceProviderFactory ( new AutofacServiceProviderFactory () )
	.ConfigureContainer<ContainerBuilder> ( startup.ConfigureContainer )
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
	  } );

startup.ConfigureServices ( builder.Services );

var webApplication = builder.Build ();

startup.Configure ( webApplication );

await webApplication.RunAsync ();