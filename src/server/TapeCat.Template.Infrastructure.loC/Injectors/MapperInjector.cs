namespace TapeCat.Template.Infrastructure.loC.Injectors;

using InjectorBuilder.Common.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class MapperInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		BootstrapTypeAdapterConfig ( typeAdapterConfig =>
		  {
			  serviceCollection.AddSingleton ( typeAdapterConfig );

			  serviceCollection.AddScoped<IMapper , ServiceMapper> ();
		  } );

		static void BootstrapTypeAdapterConfig ( Action<TypeAdapterConfig> injectTypeAdapterConfig )
		{
			TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;
			TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;

			TypeAdapterConfig.GlobalSettings.Default.AvoidInlineMapping ( value: true );

			TypeAdapterConfig.GlobalSettings.Scan (
				assemblies: new[]
				{
					Assembly.GetEntryAssembly ()!
				} );

			injectTypeAdapterConfig ( TypeAdapterConfig.GlobalSettings );
		}
	}
}