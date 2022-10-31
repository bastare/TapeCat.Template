using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Annotations;
using TapeCat.Template.Api;
using TapeCat.Template.Api.Configurations.HttpResult.Common.Extensions;
using TapeCat.Template.Contracts;
using TapeCat.Template.Contracts.HomeContracts.Query;

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

var v1ApiSet =
	webApplication.NewApiVersionSet ()
		.HasApiVersion ( apiVersion: new ( 1 , 0 ) )
		.ReportApiVersions ()
		.Build ();

webApplication.MapGet (
	"/api/home" ,
	async ( IRequestClient<GetHomeContract> getHomeRequestClient , CancellationToken cancellationToken ) =>
	  {
		  var (response, fault) =
			  await getHomeRequestClient.GetResponse<SubmitHomeContract , FaultContract> (
				new ( "Hello" ) ,
				cancellationToken );

		  return response.IsCompletedSuccessfully
			? Results.Ok ( ( await response ).Message )
			: Results.Extensions.ErrorResponse ( ( await fault ).Message.Exception );
	  } )
	.WithName ( "Get message" )
	.WithTags ( "Base" )
	.Produces ( StatusCodes.Status200OK )
	.WithMetadata ( new SwaggerOperationAttribute ( "Summary" , "Description" ) )
	.WithApiVersionSet ( v1ApiSet );

await webApplication.RunAsync ();