namespace TapeCat.Template.Infrastructure.Persistence.Uow.Interfaces;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Ef;

public interface IEfUnitOfWork<TContext, TKey> : IUnitOfWork<TKey>, IAsyncDisposable, IDisposable, ITransaction
	where TContext : DbContext
{
	new EfRepository<TModel , TKey , TContext> Repository<TModel> ()
		where TModel : class, IModel<TKey>;

	Task EnsureCreatedAsync ( CancellationToken cancellationToken = default );

	void EnsureCreated ();
}