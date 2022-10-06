namespace TapeCat.Template.Persistence.Repositories.MongoDb.Interfaces;

using Domain.Core.Models;
using MongoDB.Driver.Linq;

public interface IMongoDbRepository<TModel, TKey> : IRepository<TModel , TKey>
	where TModel : class, IModel<TKey>
{
	new IMongoQueryable<TModel>? Query { get; }
}