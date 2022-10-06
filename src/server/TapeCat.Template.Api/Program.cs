using Asp.Versioning;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TapeCat.Template.Api;

var builder = WebApplication.CreateBuilder (
	options: new ()
	{
		Args = args ,
		WebRootPath = "wwwroot"
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

var v1ApiSet =
	webApplication.NewApiVersionSet ()
		.HasApiVersion ( new ApiVersion ( 1 , 0 ) )
		.ReportApiVersions ()
		.Build ();

webApplication.Run ();