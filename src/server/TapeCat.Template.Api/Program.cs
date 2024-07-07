using Autofac;
using Autofac.Extensions.DependencyInjection;
using TapeCat.Template.Api;

var builder_ = WebApplication.CreateBuilder (
    options: new ()
    {
        Args = args ,
        WebRootPath = "webroot"
    } );

var startup_ = new Startup ( builder_.Configuration , builder_.Environment );

builder_.Host
    .UseServiceProviderFactory ( new AutofacServiceProviderFactory () )
    .ConfigureContainer<ContainerBuilder> ( startup_.ConfigureContainer )
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

startup_.ConfigureServices ( builder_.Services );

var webApplication = builder_.Build ();

startup_.Configure ( webApplication );

await webApplication.RunAsync ();