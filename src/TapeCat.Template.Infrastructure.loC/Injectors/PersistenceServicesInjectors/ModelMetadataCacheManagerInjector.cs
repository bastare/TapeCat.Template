namespace TapeCat.Template.Infrastructure.loC.Injectors.PersistenceServicesInjectors;

using Domain.Core.Models;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context.Configurations.ConfigurationBootstrapper;
using Persistence.Context.Configurations.ConfigurationBootstrapper.MetadataCache;

public sealed class ModelMetadataCacheManagerInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		var modelMetadataCacheManager = CreateModelMetadataCacheManager ();
		var ModelCreatingConfigurator = CreateModelCreatingConfigurator ( modelMetadataCacheManager );

		serviceCollection
			.AddSingleton ( modelMetadataCacheManager )
			.AddSingleton ( ModelCreatingConfigurator );

		static ModelMetadataCacheManager CreateModelMetadataCacheManager ()
			=> ModelMetadataCacheManager.Create (
				assemblies: new[]
				{
					typeof ( IModel<> ).Assembly
				} ,
				isEntityForCaching: ( modelTypeForCaching ) =>
					modelTypeForCaching.GetInterfaces ()
						.Contains ( typeof ( IModel<Guid> ) ) );

		static ModelCreatingConfigurator CreateModelCreatingConfigurator ( ModelMetadataCacheManager modelMetadataCacheManager )
			=> new ( modelMetadataCacheManager );
	}
}