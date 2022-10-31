namespace TapeCat.Template.Infrastructure.loC.Injectors;

using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

[InjectionOrder ( order: 2 )]
public sealed class LoggerFactoryInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddLogging ( loggingBuilder =>
		  {
			  loggingBuilder.AddSerilog (
				logger: CreateLogger () ,
				dispose: true );
		  } );

		static Logger CreateLogger ()
			=> new LoggerConfiguration ()
				.Enrich.FromLogContext ()
				.WriteTo.Async ( loggerSinkConfiguration =>
				  {
					  loggerSinkConfiguration.Console ();
				  } )
				.CreateLogger ();
	}
}