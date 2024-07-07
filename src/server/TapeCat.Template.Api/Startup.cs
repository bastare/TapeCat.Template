namespace TapeCat.Template.Api;

using System.IO.Compression;
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
            .Configure<BrotliCompressionProviderOptions> ( options => options.Level = CompressionLevel.Fastest )
            .Configure<ApiBehaviorOptions> ( options => options.SuppressModelStateInvalidFilter = true )
            .AddResponseCompression ( options =>
            {
                options.EnableForHttps = true;

                options.Providers.Add<BrotliCompressionProvider> ();

                options.MimeTypes = [
                    .. ResponseCompressionDefaults.MimeTypes,
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
                ];
            } )
            .InjectLayersDependency ( _configuration )
            .AddFastEndpoints ();

        if ( _webHostEnvironment.IsDevelopment () )
        {
            serviceCollection
                .SwaggerDocument ()
                .AddCors ()
                .AddSpaYarp ();
        }
    }

    public void ConfigureContainer ( ContainerBuilder containerBuilder )
    {
        containerBuilder.InjectLayersDependency ();
    }

    public void Configure ( WebApplication webApplication )
    {
        if ( _webHostEnvironment.IsProduction () )
            webApplication.UseSecureHeaders ();

        if ( _webHostEnvironment.IsDevelopment () )
            webApplication
                .UseCors ( builder =>
                    builder
                        .AllowAnyOrigin ()
                        .AllowAnyHeader ()
                        .AllowAnyMethod () );

        webApplication
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
            .UseSpaYarpMiddleware ()
            .UseRouting ()
            .UseExceptionHandler ( GlobalExceptionHandlerConfigurator.ExceptionFiltersConfigurator )
            .UseEndpoints ( endpoints =>
            {
                /*
					! If swagger doesn't work (white screen):
					1. Comment this 2 lines (Yarp mapping)
					2. Launch server & manually move to swagger page (http://localhost:5000/swagger/index.html)
					3. Uncomment it back
					4. Launch server again & don't close previously opened tab with swagger (step 1)
				*/
                if ( _webHostEnvironment.IsDevelopment () )
                    endpoints.MapSpaYarp ();

                if ( _webHostEnvironment.IsProduction () )
                    endpoints.MapFallbackToFile ( "index.html" );

                endpoints.MapFastEndpoints ();

                if ( _webHostEnvironment.IsDevelopment () )
                    webApplication.UseSwaggerGen ();
            } );
    }
}