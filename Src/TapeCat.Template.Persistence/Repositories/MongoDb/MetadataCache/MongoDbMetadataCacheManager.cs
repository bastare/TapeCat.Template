namespace TapeCat.Template.Persistence.Repositories.MongoDb.MetadataCache;

using Domain.Shared.Common.Attributes;

public sealed class MongoDbMetadataCacheManager
{
	private sealed record MongoDbCache (
		Dictionary<Type , string> CollectionNameCache ,
		Dictionary<Type , string> DatabaseNameCache )
	{
		public MongoDbCache ()
			: this (
				CollectionNameCache: new () ,
				DatabaseNameCache: new () )
		{ }
	}

	private ImmutableDictionary<Type , string> CollectionNameCache { get; } = ImmutableDictionary<Type , string>.Empty;

	private ImmutableDictionary<Type , string> DatabaseNameCache { get; } = ImmutableDictionary<Type , string>.Empty;

	private MongoDbMetadataCacheManager ( IEnumerable<Assembly> assemblies )
	{
		var (collectionNameCache, databaseNameCache) =
			CreateMongoDbCache (
				modelsTypeForCaching: ResolveModelsTypeForCaching ( assemblies ) );

		CollectionNameCache = collectionNameCache.ToImmutableDictionary ();
		DatabaseNameCache = databaseNameCache.ToImmutableDictionary ();

		static MongoDbCache CreateMongoDbCache ( IEnumerable<Type> modelsTypeForCaching )
			=> modelsTypeForCaching
				.Aggregate (
					new MongoDbCache () ,
					( mongoDbCache , modelTypeForCaching ) =>
					  {
						  var (collectionName, databaseName) = ResolveMongoDbAttribute ( modelTypeForCaching )!;

						  return mongoDbCache.Tap ( self =>
							{
								self.DatabaseNameCache.Add ( modelTypeForCaching , databaseName );
								self.CollectionNameCache.Add ( modelTypeForCaching , collectionName );
							} );
					  } );

		static IEnumerable<Type> ResolveModelsTypeForCaching ( IEnumerable<Assembly> assemblies )
			=> GetAllAssemblyTypes ( assemblies )
				.Where ( HasMongoDbAttribute );

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

		static bool HasMongoDbAttribute ( Type modelTypeForCaching )
			=> ResolveMongoDbAttribute ( modelTypeForCaching ) is not null;

		static MongoDbAttribute? ResolveMongoDbAttribute ( Type modelTypeForCaching )
			=> modelTypeForCaching.GetCustomAttribute<MongoDbAttribute> ();
	}

	public static MongoDbMetadataCacheManager Create ( IEnumerable<Assembly> assemblies )
		=> new ( assemblies );

	public string GetCachedCollectionName ( Type cachedModelType )
		=> CollectionNameCache.GetValueOrDefault ( cachedModelType ) ??
			throw new ArgumentException ( $"`{cachedModelType.Name}` has no collection name cache" );

	public string GetCachedDatabaseName ( Type cachedModelType )
		=> DatabaseNameCache.GetValueOrDefault ( cachedModelType ) ??
			throw new ArgumentException ( $"`{cachedModelType.Name}` has no database name cache" );
}