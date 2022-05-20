namespace TapeCat.Template.Persistence.Context.Configurations.ConfigurationBootstraper.MetadataCache;

public sealed class ModelMetadataCacheManager
{
	private readonly Func<Type , bool> _isEntityForCaching;

	public ImmutableList<Type> CachedModelTypes { get; } = ImmutableList<Type>.Empty;

	private ModelMetadataCacheManager ( IEnumerable<Assembly>? assemblies , Func<Type , bool>? isEntityForCaching )
	{
		NotNullOrEmpty ( assemblies );
		NotNull ( isEntityForCaching );

		_isEntityForCaching = isEntityForCaching!;

		CachedModelTypes =
			ResolveModelsTypeForCaching ( assemblies! )
				.ToImmutableList ();

		IEnumerable<Type> ResolveModelsTypeForCaching ( IEnumerable<Assembly> assemblies )
			=> assemblies
				.SelectMany ( GetAllAssemblyTypes )
				.Where ( IsEntityForCaching );

		static IEnumerable<Type> GetAllAssemblyTypes ( Assembly assembly )
			=> assembly.GetTypes ();

		bool IsEntityForCaching ( Type modelTypeForCaching )
			=> _isEntityForCaching.Invoke ( modelTypeForCaching );
	}

	public static ModelMetadataCacheManager Create ( IEnumerable<Assembly>? assemblies , Func<Type , bool>? isEntityForCaching )
		=> new ( assemblies , isEntityForCaching );
}