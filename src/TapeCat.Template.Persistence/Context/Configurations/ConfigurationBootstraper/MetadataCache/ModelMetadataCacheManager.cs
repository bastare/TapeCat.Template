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
			=> assemblies
				.SelectMany ( GetAllAssemblyTypes )
				.Where ( IsModelType );

		static IEnumerable<Type> GetAllAssemblyTypes ( Assembly assembly )
			=> assembly.GetTypes ();

		bool IsModelType ( Type modelTypeForCaching )
			=> _domainModelFilter.Invoke ( modelTypeForCaching );
	}

	public static ModelMetadataCacheManager Create ( IEnumerable<Assembly>? assemblies , Func<Type , bool>? domainModelFilter )
		=> new ( assemblies , domainModelFilter );
}