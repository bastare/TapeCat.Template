namespace TapeCat.Template.Infrastructure.Persistence.Uow.Interfaces;

using Domain.Core.Models;
using Repositories;

public interface IUnitOfWork<TKey>
{
	IRepository<TModel , TKey> Repository<TModel> ()
		where TModel : class, IModel<TKey>;
}