namespace TapeCat.Template.Persistence.Repositories.MongoDb;

using Common.Exceptions;
using Common.Extensions;
using Domain.Core.Models;
using Domain.Shared.Common.Extensions;
using Mapster;
using MetadataCache;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pagination;

public sealed class MongoDbRepository<TModel, TKey> : IRepository<TModel , TKey>
	where TModel : class, IModel<TKey>
{
	private readonly TypeAdapterConfig _typeAdapterConfig;

	private readonly MongoClient _mongoClient;

	public IMongoCollection<TModel> ModelCollection { get; }

	public IMongoQueryable<TModel> ModelQuery => ModelCollection.AsQueryable ();

	public MongoDbRepository (
		TypeAdapterConfig typeAdapterConfig ,
		MongoClient mongoClient ,
		MongoDbMetadataCacheManager mongoDbMetadataCacheManager )
	{
		_typeAdapterConfig = typeAdapterConfig;
		_mongoClient = mongoClient;

		ModelCollection = ResolveCollection ();

		IMongoCollection<TModel> ResolveCollection ()
		{
			var (databaseName, collectionName) =
				ResolveConnectionData ( mongoDbMetadataCacheManager );

			return _mongoClient.GetDatabase ( databaseName )
				.GetCollection<TModel> ( collectionName );

			static (string DatabaseName, string CollectionName) ResolveConnectionData ( MongoDbMetadataCacheManager mongoDbMetadataCacheManager )
				=> (
					DatabaseName: mongoDbMetadataCacheManager.GetCachedDatabaseName ( cachedModelType: typeof ( TModel ) ),
					CollectionName: mongoDbMetadataCacheManager.GetCachedCollectionName ( cachedModelType: typeof ( TModel ) )
				);
		}
	}

	public async Task<TModel> AddAsync ( TModel model , CancellationToken cancellationToken = default )
		=> await model.TapAsync ( async self =>
			await ModelCollection.InsertOneAsync (
				self ,
				options: new () { BypassDocumentValidation = true } ,
				cancellationToken ) )!;

	public async Task<List<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await ModelQuery.Where ( predicate )
			.ToListAsync ( cancellationToken );

	public async Task<PagedList<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default )
		=> await ModelQuery.Where ( predicate )
			.ToPagedListAsync ( offset , limit , cancellationToken );

	public async Task<List<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await ModelQuery.Where ( predicate )
			.ProjectToType<TModel , TMappable> ( _typeAdapterConfig )
			.ToListAsync ( cancellationToken );

	public async Task<PagedList<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default )
		=> await ModelQuery.Where ( predicate )
			.ProjectToType<TModel , TMappable> ( _typeAdapterConfig )
			.ToPagedListAsync ( offset , limit , cancellationToken );

	public async Task<TModel?> FindByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await ModelQuery.SingleOrDefaultAsync ( predicate , cancellationToken );

	public async Task<TMappable?> FindByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await ModelQuery.Where ( predicate )
			.ProjectToType<TModel , TMappable> ( _typeAdapterConfig )
			.SingleOrDefaultAsync ( cancellationToken );

	public async Task<List<TModel>> GetAllAsync ( CancellationToken cancellationToken = default )
		=> await ModelQuery.ToListAsync ( cancellationToken );

	public async Task<List<TTransform>> GetAllAsync<TTransform> ( Expression<Func<TModel , TTransform>> selector , CancellationToken cancellationToken = default )
		=> await ModelQuery.Select ( selector )
			.ToListAsync ( cancellationToken );

	public async Task<PagedList<TModel>> GetAllAsync ( int offset , int limit , CancellationToken cancellationToken = default )
		=> await ModelQuery.ToPagedListAsync ( offset , limit , cancellationToken );

	public async Task<List<TMappable>> GetAllAsync<TMappable> ( CancellationToken cancellationToken = default )
		=> await ModelQuery.ProjectToType<TModel , TMappable> ( _typeAdapterConfig )
			.ToListAsync ( cancellationToken );

	public async Task<PagedList<TMappable>> GetAllAsync<TMappable> ( int offset , int limit , CancellationToken cancellationToken = default )
		=> await ModelQuery.ProjectToType<TModel , TMappable> ( _typeAdapterConfig )
			.ToPagedListAsync ( offset , limit , cancellationToken );

	public async Task<TModel?> GetAsync ( TKey id , CancellationToken cancellationToken = default )
		=> await ModelQuery.SingleOrDefaultAsync (
			model => model.Id!.Equals ( id ) ,
			cancellationToken );

	public async Task<TMappable?> GetAsync<TMappable> ( TKey id , CancellationToken cancellationToken = default )
		=> await ModelQuery.Where ( model => model.Id!.Equals ( id ) )
			.ProjectToType<TModel , TMappable> ( _typeAdapterConfig )
			.SingleOrDefaultAsync ( cancellationToken );

	public async Task<bool> IsExistAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await ModelQuery.AnyAsync ( predicate , cancellationToken );

	public async Task<TModel?> RemoveAsync ( TModel model , CancellationToken cancellationToken = default )
		=> await model.TapAsync ( async self =>
		  {
			  var result =
				  await ModelCollection.DeleteOneAsync (
					  filter: ( collectionModel ) => collectionModel.Id!.Equals ( self.Id ) ,
					  cancellationToken );

			  if ( result is { DeletedCount: <= 0 } )
				  throw new RepositoryException ( "Entity wasn`t removed" );
		  } );

	public async Task<TModel?> RemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await ModelCollection.DeleteOneAsync ( predicate , cancellationToken )
			.TapAsync ( async resultTask =>
			{
				if ( await resultTask is { DeletedCount: <= 0 } )
					throw new RepositoryException ( "Entity wasn`t removed" );

				// FIXME: Should be first parameter of `predicate` expression
				return default ( TModel? );
			} );

	public async Task<TModel> UpdateAsync ( TModel model , CancellationToken cancellationToken = default )
		=> await model.TapAsync ( async self =>
		  {
			  await ModelCollection.ReplaceOneAsync (
				  filter: collectionModel => collectionModel.Id!.Equals ( model.Id ) ,
				  replacement: self ,
				  options: new ReplaceOptions { IsUpsert = true } ,
				  cancellationToken );
		  } );
}