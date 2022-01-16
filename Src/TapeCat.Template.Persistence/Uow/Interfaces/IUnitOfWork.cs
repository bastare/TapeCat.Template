namespace TapeCat.Template.Persistence.Uow.Interfaces;

using Domain.Core.Models;
using Repositories.Interfaces;
using System;

public interface IUnitOfWork<TKey> : IDisposable, IAsyncDisposable, ITransaction
{
	IRepository<TModel , TKey> Repository<TModel> ()
		where TModel : class, IModel<TKey>;
}