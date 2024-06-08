namespace TapeCat.Template.Infrastructure.Persistence.Uow;

using Common.Exceptions;
using Domain.Core.Models;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Persistence.Repositories.Ef;

public sealed class EfUnitOfWork<TContext, TKey> ( TContext context , TypeAdapterConfig typeAdapterConfig ) : IEfUnitOfWork<TContext , TKey>
	where TContext : DbContext
{
	private readonly TContext _context = context;

	private readonly TypeAdapterConfig _typeAdapterConfig = typeAdapterConfig;

	public EfRepository<TModel , TKey , TContext> Repository<TModel> ()
		where TModel : class, IModel<TKey>
			=> new ( _context , _typeAdapterConfig );

	IRepository<TModel , TKey> IUnitOfWork<TKey>.Repository<TModel> ()
		=> Repository<TModel> ();

	public async Task CommitAsync ( CancellationToken cancellationToken = default )
	{
		if ( !await TryCommitAsync ( cancellationToken ) )
			throw new DataWasNotSavedException ();
	}

	public async Task<bool> TryCommitAsync ( CancellationToken cancellationToken = default )
		=> await SaveChangesAsync ( cancellationToken ) != 0;

	public Task<int> SaveChangesAsync ( CancellationToken cancellationToken = default )
		=> _context.SaveChangesAsync ( cancellationToken );

	public void Dispose ()
	{
		_context.Dispose ();
	}

	public ValueTask DisposeAsync ()
		=> _context.DisposeAsync ();
}