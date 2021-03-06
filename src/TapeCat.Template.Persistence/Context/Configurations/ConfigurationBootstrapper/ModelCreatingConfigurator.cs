namespace TapeCat.Template.Persistence.Context.Configurations.ConfigurationBootstrapper;

using Common.Extensions;
using MetadataCache;
using Microsoft.EntityFrameworkCore;

public sealed class ModelCreatingConfigurator
{
	private readonly ModelMetadataCacheManager _modelMetadataCacheManager;

	private ModelCreatingConfigurationOption ModelCreatingConfigurationOption { get; } = new () { JsonFieldTypeName = "nvarchar(max)" };

	public ModelCreatingConfigurator (
		ModelMetadataCacheManager modelMetadataCacheManager ,
		Func<ModelCreatingConfigurationOption , ModelCreatingConfigurationOption> setupOption )
	{
		_modelMetadataCacheManager = modelMetadataCacheManager;
		ModelCreatingConfigurationOption = InvokeSetupOption ( setupOption );

		ModelCreatingConfigurationOption InvokeSetupOption ( Func<ModelCreatingConfigurationOption , ModelCreatingConfigurationOption> setupOption )
			=> setupOption.Invoke ( arg: ModelCreatingConfigurationOption );
	}

	public ModelCreatingConfigurator ( ModelMetadataCacheManager modelMetadataCacheManager )
	{
		_modelMetadataCacheManager = modelMetadataCacheManager;
	}

	public void Configure ( ModelBuilder modelBuilder )
	{
		_modelMetadataCacheManager.CachedModelTypes
			.AsParallel ()
			.ForAll ( cachedModelTypes =>
			  {
				  ConfigureJsonSerializableFields ( modelBuilder , cachedModelTypes );
			  } );

		void ConfigureJsonSerializableFields ( ModelBuilder modelBuilder , Type cachedModelTypes )
		{
			modelBuilder.ConfigureJsonSerializableFields ( cachedModelTypes , ModelCreatingConfigurationOption.JsonFieldTypeName );
		}
	}
}