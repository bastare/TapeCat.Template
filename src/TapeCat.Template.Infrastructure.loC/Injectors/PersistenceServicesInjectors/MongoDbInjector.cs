namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Domain.Core.Models;
using Infrastructure.loC.Boostrapers;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Persistence.Repositories.MongoDb.MetadataCache;
using Persistence.Uow;
using Persistence.Uow.Interfaces;

[InjectionOrder ( order: uint.MaxValue )]
public sealed class MongoDbInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		var modelAssemblies = new Assembly[]
		{
			typeof ( IModel<> ).Assembly
		};

		BoostrapLookupSerializer ( modelAssemblies );
		InjectMongoDbModelCache ( serviceCollection , modelAssemblies );

		AddCamelCaseConvention ();

		serviceCollection.AddScoped ( _ => new MongoClient (
			connectionString: configuration.GetConnectionString ( string.Empty ) ) );

		serviceCollection.AddScoped<IUnitOfWork<ObjectId> , MongoDbUnitOfWork> ();
		serviceCollection.AddScoped<IMongoDbUnitOfWork<ObjectId> , MongoDbUnitOfWork> ();

		static void BoostrapLookupSerializer ( Assembly[] modelAssemblies )
		{
			MongoClientBoostraper.BoostrapLookupSerializerByDtoEntities ( modelAssemblies );
		}

		static void InjectMongoDbModelCache ( IServiceCollection serviceCollection , Assembly[] modelAssemblies )
		{
			serviceCollection.AddSingleton ( MongoDbMetadataCacheManager.Create ( modelAssemblies ) );
		}

		static void AddCamelCaseConvention ()
		{
			ConventionRegistry.Register (
				name: "camelCase" ,
				conventions: new ConventionPack { new CamelCaseElementNameConvention () } ,
				filter: _ => true );
		}
	}
}