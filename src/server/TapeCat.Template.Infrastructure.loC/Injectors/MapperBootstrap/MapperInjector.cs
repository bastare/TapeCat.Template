namespace TapeCat.Template.Infrastructure.loC.Injectors.MapperBootstrap;

using AgileObjects.NetStandardPolyfills;
using Configurations;
using InjectorBuilder.Common.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class MapperInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		BootstrapTypeAdapterConfig ( typeAdapterConfig =>
		  {
			  serviceCollection.TryAddSingleton ( typeAdapterConfig );

			  serviceCollection.TryAddScoped<IMapper , ServiceMapper> ();
		  } );

		static void BootstrapTypeAdapterConfig ( Action<TypeAdapterConfig> injectTypeAdapterConfig )
		{
			TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;
			TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;

			TypeAdapterConfig.GlobalSettings.Default.AvoidInlineMapping ( value: true );

			TypeAdapterConfig.GlobalSettings.Scan (
				assemblies: [
					Assembly.GetEntryAssembly ()!,
					typeof (PaginationConfiguration).GetAssembly()
				] );

			injectTypeAdapterConfig ( TypeAdapterConfig.GlobalSettings );
		}
	}
}