namespace TapeCat.Template.Infrastructure.loC.Injectors.PersistenceServicesInjectors;

using Domain.Core.Models;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistence.Context.Configurations.ConfigurationBootstrapper;
using Persistence.Context.Configurations.ConfigurationBootstrapper.MetadataCache;

public sealed class ModelMetadataCacheManagerInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		var modelMetadataCacheManager = CreateModelMetadataCacheManager ();
		var modelCreatingConfigurator = CreateModelCreatingConfigurator ( modelMetadataCacheManager );

		serviceCollection.TryAddSingleton ( modelMetadataCacheManager );
		serviceCollection.TryAddSingleton ( modelCreatingConfigurator );

		static ModelMetadataCacheManager CreateModelMetadataCacheManager ()
			=> ModelMetadataCacheManager.Create (
				assemblies:
				[
					typeof ( IModel ).Assembly
				] ,
				isEntityForCaching: ( modelTypeForCaching ) =>
					modelTypeForCaching.GetInterfaces ()
						.Contains ( typeof ( IModel ) ) );

		static ModelCreatingConfigurator CreateModelCreatingConfigurator ( ModelMetadataCacheManager modelMetadataCacheManager )
			=> new ( modelMetadataCacheManager );
	}
}