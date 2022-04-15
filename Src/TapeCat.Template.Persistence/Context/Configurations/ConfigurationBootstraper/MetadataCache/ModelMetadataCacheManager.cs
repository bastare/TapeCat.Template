namespace TapeCat.Template.Persistence.Context.Configurations.ConfigurationBootstraper.MetadataCache;

public sealed class ModelMetadataCacheManager
{
	private readonly Func<Type , bool> _domainModelFilter;

	public ImmutableList<Type> CachedModelTypes { get; } = ImmutableList<Type>.Empty;

	private ModelMetadataCacheManager ( IEnumerable<Assembly>? assemblies , Func<Type , bool>? domainModelFilter )
	{
		NotNullOrEmpty ( assemblies );
		NotNull ( domainModelFilter );

		_domainModelFilter = domainModelFilter!;

		CachedModelTypes =
			ResolveModelsTypeForCaching ( assemblies! )
				.ToImmutableList ();

		IEnumerable<Type> ResolveModelsTypeForCaching ( IEnumerable<Assembly> assemblies )
			=> GetAllAssemblyTypes ( assemblies )
				.Where ( IsModelType );

		static HashSet<Type> GetAllAssemblyTypes ( IEnumerable<Assembly> assemblies )
			=> assemblies
				.Aggregate (
					new HashSet<Type> () ,
					( types , assembly ) =>
					  {
						  types.UnionWith (
							  other: assembly.GetTypes () );

						  return types;
					  } );

		bool IsModelType ( Type modelTypeForCaching )
			=> _domainModelFilter.Invoke ( modelTypeForCaching );
	}

	public static ModelMetadataCacheManager Create ( IEnumerable<Assembly>? assemblies , Func<Type , bool>? domainModelFilter )
		=> new ( assemblies , domainModelFilter );
}