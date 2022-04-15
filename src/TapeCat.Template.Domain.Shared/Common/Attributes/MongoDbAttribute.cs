namespace TapeCat.Template.Domain.Shared.Common.Attributes;

[AttributeUsage ( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface , AllowMultiple = false )]
public sealed class MongoDbAttribute : Attribute
{
	public string CollectionName { get; }

	public string DatabaseName { get; }

	public MongoDbAttribute ( string? collectionName , string? databaseName )
	{
		NotNullOrEmpty ( collectionName , message: "Collection name is empty" );
		NotNullOrEmpty ( databaseName , message: "Database name is empty" );

		CollectionName = collectionName!;
		DatabaseName = databaseName!;
	}

	public void Deconstruct ( out string collectionName , out string databaseName )
	{
		collectionName = CollectionName;
		databaseName = DatabaseName;
	}
}