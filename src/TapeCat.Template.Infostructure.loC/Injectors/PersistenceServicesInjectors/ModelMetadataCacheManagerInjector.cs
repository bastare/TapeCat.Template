namespace TapeCat.Template.Infostructure.loC.Injectors.PersistenceServicesInjectors;

using Domain.Core.Models;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context.Configurations.ConfigurationBootstraper;
using Persistence.Context.Configurations.ConfigurationBootstraper.MetadataCache;

public sealed class ModelMetadataCacheManagerInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection
			.AddSingleton ( implementationInstance: CreateModelMetadataCacheManager () )
			.AddSingleton ( CreateModelCreatingConfigurator );

		static ModelMetadataCacheManager CreateModelMetadataCacheManager ()
			=> ModelMetadataCacheManager.Create (
				assemblies: new[]
				{
					typeof ( IModel<> ).Assembly
				} ,
				domainModelFilter: ( modelTypeForCaching ) =>
					modelTypeForCaching.GetInterfaces ()
						.Contains ( typeof ( IModel<Guid> ) ) );

		static ModelCreatingConfigurator CreateModelCreatingConfigurator ( IServiceProvider serviceCollection )
			=> new ( modelMetadataCacheManager: serviceCollection.GetRequiredService<ModelMetadataCacheManager> () );
	}
}
