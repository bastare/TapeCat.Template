namespace TapeCat.Template.Infostructure.loC.Injectors;

using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class OptionsInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
	}
}