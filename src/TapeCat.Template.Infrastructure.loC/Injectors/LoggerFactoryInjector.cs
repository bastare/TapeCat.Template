namespace TapeCat.Template.Infrastructure.loC.Injectors;

using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[InjectionOrder ( order: 2 )]
public sealed class LoggerFactoryInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		serviceCollection.AddSingleton (
			LoggerFactory.Create ( loggingBuilder =>
			  {
				  loggingBuilder.AddConsole ();
			  } ) );
	}
}