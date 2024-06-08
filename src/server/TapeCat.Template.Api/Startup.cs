namespace TapeCat.Template.Api;

using System.IO.Compression;
using System.Linq;
using Asp.Versioning;
using Autofac;
using FastEndpoints;
using FastEndpoints.Swagger;
using HeyRed.Mime;
using Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;
using Infrastructure.CrossCutting.Projections.DependencyInjectionBootstrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pipes.SecurityPipes;
using Common.Extensions;

public sealed class Startup ( IConfiguration configuration , IWebHostEnvironment webHostEnvironment )
{
	private readonly IConfiguration _configuration = configuration;

	private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

	public void ConfigureServices ( IServiceCollection serviceCollection )
	{
		serviceCollection.AddApiVersioning ( setupAction =>
		{
			setupAction.DefaultApiVersion = new ( 1 , 0 );
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
					second: [
						MimeTypesMap.GetMimeType("svg"),
						MimeTypesMap.GetMimeType("gif"),
						MimeTypesMap.GetMimeType("html"),
						MimeTypesMap.GetMimeType("txt"),
						MimeTypesMap.GetMimeType("css"),
						MimeTypesMap.GetMimeType("png"),
						MimeTypesMap.GetMimeType("jpg"),
						MimeTypesMap.GetMimeType("js"),
						MimeTypesMap.GetMimeType("json"),
						MimeTypesMap.GetMimeType("ico"),
						MimeTypesMap.GetMimeType("woff"),
						MimeTypesMap.GetMimeType("woff2")
					]
				);
			} )
			.InjectLayersDependency ( _configuration )
			.AddFastEndpoints ();

		if ( WebHostEnvironmentExtensions.IsDevelopment ( _webHostEnvironment ) )
		{
			serviceCollection
				.SwaggerDocument ()
				.AddCors()
				.AddSpaYarp ();
		}
	}

	public void ConfigureContainer ( ContainerBuilder containerBuilder )
	{
		containerBuilder.InjectLayersDependency ();
	}

	public void Configure ( IApplicationBuilder applicationBuilder )
	{
		if ( WebHostEnvironmentExtensions.IsProduction ( _webHostEnvironment ) )
		{
			applicationBuilder
				.UseHttpsRedirection ()
				.UseHsts ( hsts =>
				{
					hsts.MaxAge ( days: 365 ).IncludeSubdomains ();
				} )
				.UseXContentTypeOptions ()
				.UsePermissionsPolicy (
					siteUrl => [
						$"fullscreen=(self {siteUrl})",
						$"geolocation=(self {siteUrl})",
						$"payment=(self {siteUrl})",
						"camera=()",
						"microphone=()",
						"usb=()"
					]
				)
				.UseXfo ( xfo =>
				{
					xfo.SameOrigin ();
				} )
				.UseReferrerPolicy ( options =>
				{
					options.NoReferrer ();
				} )
				.UseXXssProtection ( options =>
				{
					options.EnabledWithBlockMode ();
				} )
				.UseCsp ( options =>
				{
					options
						.StyleSources ( configure =>
						{
							configure
								.Self ()
								.CustomSources (
									"www.google.com" ,
									"platform.twitter.com" ,
									"cdn.syndication.twimg.com" ,
									"fonts.googleapis.com"
								)
								.UnsafeInline ();
						} )
						.ScriptSources ( configure =>
						{
							configure
								.Self ()
								.CustomSources (
									"www.google.com" ,
									"cse.google.com" ,
									"cdn.syndication.twimg.com" ,
									"platform.twitter.com" ,
									"https://www.google-analytics.com" ,
									"https://connect.facebook.net" ,
									"https://www.youtube.com"
								)
								.UnsafeInline ()
								.UnsafeEval ();
						} );
				} );
		}

		if ( WebHostEnvironmentExtensions.IsDevelopment ( _webHostEnvironment ) )
		{
			applicationBuilder
				.UseCors ( builder =>
				{
					builder
						.AllowAnyOrigin ()
						.AllowAnyHeader ()
						.AllowAnyMethod ();
				} );
		}

		applicationBuilder
			.UseResponseCompression ()
			.UseDefaultFiles ()
			.UseStaticFiles (
				options: new ()
				{
					OnPrepareResponse = ( context ) =>
					{
						var headers = context.Context.Response.GetTypedHeaders ();

						headers.CacheControl = new ()
						{
							Public = true ,
							MaxAge = TimeSpan.FromDays ( 30 )
						};
					}
				}
			)
			.UseRedirectValidation ()
			.UseRouting ()
			.UseExceptionHandler ( GlobalExceptionHandlerConfigurator.ExceptionFiltersConfigurator )
			.UseEndpoints ( endpoints =>
			{
				if ( WebHostEnvironmentExtensions.IsDevelopment ( _webHostEnvironment ) )
					endpoints.MapSpaYarp ();

				endpoints.MapFallbackToFile (
					filePath: Path.Combine ( _webHostEnvironment.WebRootPath , "index.html" )
				);

				if ( WebHostEnvironmentExtensions.IsDevelopment ( _webHostEnvironment ) )
					applicationBuilder.UseSwaggerGen ();
			} );
	}
}