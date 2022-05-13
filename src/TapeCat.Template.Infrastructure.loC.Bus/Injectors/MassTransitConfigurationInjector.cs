namespace TapeCat.Template.Infrastructure.loC.Bus.Injectors;

using InjectorBuilder.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class MassTransitConfigurationInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		serviceCollection.TryAddSingleton ( KebabCaseEndpointNameFormatter.Instance );
	}
}