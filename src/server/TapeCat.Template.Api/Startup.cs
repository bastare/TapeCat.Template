namespace TapeCat.Template.Api;

using Asp.Versioning;
using Autofac;
using Common.Extensions;
using HeyRed.Mime;
using Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;
using Infrastructure.CrossCutting.Configurators.SwaggerConfigurators;
using Infrastructure.CrossCutting.Projections.DependencyInjectionBootstrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipes.SecurityPipes;
using System.IO.Compression;
using System.Linq;
using Environments = Common.Constants.Environments;

public sealed class Startup
{
	private readonly IConfiguration _configuration;

	private readonly IWebHostEnvironment _webHostEnvironment;

	public Startup (
		IConfiguration configuration ,
		IWebHostEnvironment webHostEnvironment )
	{
		_configuration = configuration;
		_webHostEnvironment = webHostEnvironment;
	}

	public void ConfigureServices ( IServiceCollection serviceCollection )
	{
		serviceCollection
			.AddApiVersioning ( setupAction =>
			  {
				  setupAction.DefaultApiVersion = new ApiVersion ( 1 , 0 );
				  setupAction.ReportApiVersions = true;
				  setupAction.AssumeDefaultVersionWhenUnspecified = true;
				  setupAction.ApiVersionReader = new QueryStringApiVersionReader ( "v" );
			  } );

		serviceCollection
			.Configure<BrotliCompressionProviderOptions> ( options =>
			  {
				  options.Level = CompressionLevel.Fastest;
			  } )

			.Configure<ApiBehaviorOptions> ( options =>
			  {
				  options.SuppressModelStateInvalidFilter = true;
			  } )

			.AddResponseCompression ( options =>
			  {
				  options.EnableForHttps = true;

				  options.Providers.Add<BrotliCompressionProvider> ();

				  options.MimeTypes = Enumerable.Concat (
					first: ResponseCompressionDefaults.MimeTypes ,
					second: new[]
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
			  } )

			.AddCaching ()

			.InjectLayersDependency ( _configuration );

		if ( _webHostEnvironment.IsEnvironment ( Environments.Development ) )
		{
			serviceCollection
				.AddCors ()
				.AddEndpointsApiExplorer ()
				.AddSwaggerGen ( SwaggerConfigurator.SwaggerDIConfigurator )
				.AddSwaggerGenNewtonsoftSupport ();
		}
	}

	public void ConfigureContainer ( ContainerBuilder containerBuilder )
	{
		containerBuilder.InjectLayersDependency ();
	}

	public void Configure ( IApplicationBuilder applicationBuilder )
	{
		if ( !_webHostEnvironment.IsEnvironment ( Environments.Development ) )
		{
			applicationBuilder
				.UseHttpsRedirection ()
				.UseHsts ( hsts =>
				  {
					  hsts.MaxAge ( days: 365 )
						  .IncludeSubdomains ();
				  } )
				.UseXContentTypeOptions ()
				.UsePermissionsPolicy ( siteUrl =>
					new[]
					{
						$"fullscreen=(self {siteUrl})" ,
						$"geolocation=(self {siteUrl})" ,
						$"payment=(self {siteUrl})" ,
						"camera=()" ,
						"microphone=()" ,
						"usb=()"
					} )
				.UseXfo ( xfo => { xfo.SameOrigin (); } )
				.UseReferrerPolicy ( options => { options.NoReferrer (); } )
				.UseXXssProtection ( options => { options.EnabledWithBlockMode (); } )
				.UseCsp ( options =>
				  {
					  options
						  .StyleSources ( configure =>
							  {
								  configure.Self ()
									  .CustomSources (
										 "www.google.com" ,
										 "platform.twitter.com" ,
										 "cdn.syndication.twimg.com" ,
										 "fonts.googleapis.com" )
									  .UnsafeInline ();
							  } )
						  .ScriptSources ( configure =>
							  {
								  configure.Self ()
									  .CustomSources (
										"www.google.com" ,
										"cse.google.com" ,
										"cdn.syndication.twimg.com" ,
										"platform.twitter.com" ,
										"https://www.google-analytics.com" ,
										"https://connect.facebook.net" ,
										"https://www.youtube.com" )
									  .UnsafeInline ()
									  .UnsafeEval ();
							  } );
				  } );
		}

		if ( _webHostEnvironment.IsEnvironment ( Environments.Development ) )
		{
			applicationBuilder
				.UseCors ( builder =>
				  {
					  builder
						  .AllowAnyOrigin ()
						  .AllowAnyHeader ()
						  .AllowAnyMethod ();
				  } )
				.UseDeveloperExceptionPage ()
				.UseSwagger ()
				.UseSwaggerUI ( SwaggerConfigurator.SwaggerUIConfigurator );
		}

		applicationBuilder
			.UseResponseCompression ()
			.UseDefaultFiles ()
			.UseStaticFiles ()
			.UseRedirectValidation ()
			.UseRouting ()
			.UseExceptionHandler ( GlobalExceptionHandlerConfigurator.ExceptionFiltersConfigurator )
			.UseEndpoints ( endpoints =>
			  {
				  endpoints.MapFallbackToFile ( filePath: Path.Combine ( _webHostEnvironment.WebRootPath , "index.html" ) );
			  } );
	}
}