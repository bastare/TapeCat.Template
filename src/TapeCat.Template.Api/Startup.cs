namespace TapeCat.Template.Api;

using Autofac;
using Common.Extensions;
using Configurations.RouteEndpointConfiguration;
using Configurations.StartupConfiguration;
using Controllers;
using Filters.Actions.Global;
using FluentValidation.AspNetCore;
using HeyRed.Mime;
using Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;
using Infrastructure.CrossCutting.Configurators.FluentValidationConfigurators;
using Infrastructure.CrossCutting.Configurators.SwaggerConfigurators;
using Infrastructure.CrossCutting.Projections.DependencyInjectionBootstrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pipes;
using Pipes.SecurityPipes;
using System.IO.Compression;
using System.Linq;

public sealed class Startup
{
	private readonly IConfiguration _configuration;

	private readonly IWebHostEnvironment _webHostEnvironment;

	public Startup ( IConfiguration configuration , IWebHostEnvironment webHostEnvironment )
	{
		_configuration = configuration;
		_webHostEnvironment = webHostEnvironment;
	}

	public void ConfigureServices ( IServiceCollection services )
	{
		services
			.AddControllers ( configure =>
			  {
				  configure.Conventions.Add ( new PluralFormResourceNameConvention () );
				  configure.Conventions.Add ( new KebabEndpointConvention () );

				  configure.Filters.Add<ValidationFilter> ( order: 1 );
			  } )

			.AddFluentValidation ( FluentValidationConfigurator.FluentValidationMvcConfigurator );

		services
			.AddApiVersioning ( setupAction =>
			  {
				  setupAction.DefaultApiVersion = new ApiVersion ( 1 , 0 );
				  setupAction.AssumeDefaultVersionWhenUnspecified = true;
				  setupAction.ReportApiVersions = true;
				  setupAction.ApiVersionReader = ApiVersionReader.Combine (
					  new HeaderApiVersionReader ( "X-Version" ) ,
					  new MediaTypeApiVersionReader ( "ver" ) );
			  } )

			.Configure<BrotliCompressionProviderOptions> ( options =>
			  {
				  options.Level = CompressionLevel.Optimal;
			  } )

			.Configure<ApiBehaviorOptions> ( options =>
			  {
				  options.SuppressModelStateInvalidFilter = true;
			  } )

			.AddResponseCompression ( options =>
			  {
				  options.EnableForHttps = true;

				  options.Providers.Add<BrotliCompressionProvider> ();

				  options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat (
					new[]
					{
						MimeTypesMap.GetMimeType ( "svg" ) ,
						MimeTypesMap.GetMimeType ( "gif" ) ,
						MimeTypesMap.GetMimeType ( "html" ) ,
						MimeTypesMap.GetMimeType ( "txt" ) ,
						MimeTypesMap.GetMimeType ( "css" ) ,
						MimeTypesMap.GetMimeType ( "png" ) ,
						MimeTypesMap.GetMimeType ( "jpg" ) ,
						MimeTypesMap.GetMimeType ( "js" ) ,
						MimeTypesMap.GetMimeType ( "json" ) ,
						MimeTypesMap.GetMimeType ( "ico" ) ,
						MimeTypesMap.GetMimeType ( "woff" ) ,
						MimeTypesMap.GetMimeType ( "woff2" )
					} );
			  } );

		services.AddCaching ();

		services.AddSignalR ();

		services.InjectLayersDependency ( _configuration );

		services.AddDependency (
			_webHostEnvironment ,
			_configuration ,
			environmentConfigurations: new ()
			{
				[ "Production" ] = ( _ , _ , _ ) => { } ,

				[ "Development" ] = ( _ , _ , services ) =>
					services.AddSwaggerGen ( SwaggerConfigurator.SwaggerDIConfigurator )
						.AddSwaggerGenNewtonsoftSupport () ,

				[ "Staging" ] = ( _ , _ , _ ) => { }
			} );
	}

	public void ConfigureContainer ( ContainerBuilder containerBuilder )
	{
		containerBuilder.InjectLayersDependency ();
	}

	public void Configure ( IApplicationBuilder applicationBuilder )
	{
		applicationBuilder.UseServices (
			_webHostEnvironment ,
			_configuration ,

			environmentConfigurations: new ()
			{
				[ "Production" ] = ( webHostEnvironment , _ , applicationBuilder ) =>
					applicationBuilder.UseSecurityHeaders ( webHostEnvironment ) ,

				[ "Development" ] = ( _ , _ , applicationBuilder ) =>
					  applicationBuilder
						  .UseCors ( builder =>
							{
								builder
									.AllowAnyOrigin ()
									.AllowAnyHeader ()
									.AllowAnyMethod ();
							} )
						  .UseAccessControlExposeHeaders ()
						  .UseDeveloperExceptionPage ()
						  .UseSwagger ()
						  .UseSwaggerUI ( SwaggerConfigurator.SwaggerUIConfigurator ) ,

				[ "Staging" ] = ( _ , _ , _ ) => { }
			} ,

			generalConfiguration: ( _ , _ , applicationBuilder ) =>
				applicationBuilder
					.UseResponseCompression ()
					.UseDefaultFiles ()
					.UseStaticFiles ()
					.UseRedirectValidation ()
					.UseRouting ()
					.UseAuthentication ()
					.UseAuthorization ()
					.UseExceptionHandler ( GlobalExceptionHandlerConfigurator.ExceptionFiltersConfigurator )
					.UseEndpoints ( endpoints =>
					  {
						  endpoints.MapControllers ();

						  endpoints.MapFallbackToController (
							 action: nameof ( FallbackController.Index ) ,
							 controller: nameof ( FallbackController ).Replace ( "Controller" , string.Empty ) );
					  } )
			);
	}
}