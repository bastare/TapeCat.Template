namespace TapeCat.Template.Persistence.Uow;

using Domain.Core.Models;
using Interfaces;
using Mapster;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories;
using Repositories.MongoDb;
using Repositories.MongoDb.MetadataCache;

public sealed class MongoDbUnitOfWork : IMongoDbUnitOfWork<ObjectId>
{
	private readonly MongoClient _mongoClient;

	private readonly TypeAdapterConfig _typeAdapterConfig;

	private readonly MongoDbMetadataCacheManager _mongoDbMetadataCacheManager;

	public MongoDbUnitOfWork (
		MongoClient mongoClient ,
		TypeAdapterConfig typeAdapterConfig ,
		MongoDbMetadataCacheManager mongoDbMetadataCacheManager )
	{
		_mongoClient = mongoClient;
		_typeAdapterConfig = typeAdapterConfig;
		_mongoDbMetadataCacheManager = mongoDbMetadataCacheManager;
	}

	public MongoDbRepository<TModel , ObjectId> Repository<TModel> ()
		where TModel : class, IModel<ObjectId>
			=> new ( _typeAdapterConfig , _mongoClient , _mongoDbMetadataCacheManager );

	IRepository<TModel , ObjectId> IUnitOfWork<ObjectId>.Repository<TModel> ()
		=> Repository<TModel> ();
}