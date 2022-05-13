namespace TapeCat.Template.Infrastructure.loC.Bus.Injectors;

using InjectorBuilder.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class MassTransitInMemoryInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddMediator ( _ =>
		  {
		  } );
	}
}