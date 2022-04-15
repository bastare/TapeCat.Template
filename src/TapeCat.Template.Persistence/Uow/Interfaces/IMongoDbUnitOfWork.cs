namespace TapeCat.Template.Persistence.Uow.Interfaces;

using Domain.Core.Models;
using Repositories.MongoDb;

public interface IMongoDbUnitOfWork<TKey> : IUnitOfWork<TKey>
{
	new MongoDbRepository<TModel , TKey> Repository<TModel> ()
		where TModel : class, IModel<TKey>;
}