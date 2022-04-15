namespace TapeCat.Template.Persistence.Uow;

using Common.Exceptions;
using Domain.Core.Models;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Persistence.Repositories.Ef;

public sealed class EfUnitOfWork<TContext, TKey> : IEfUnitOfWork<TContext , TKey>
	where TContext : DbContext
{
	private readonly TContext _context;

	private readonly TypeAdapterConfig _typeAdapterConfig;

	public EfUnitOfWork ( TContext context , TypeAdapterConfig typeAdapterConfig )
	{
		_context = context;
		_typeAdapterConfig = typeAdapterConfig;
	}

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

	public async Task<int> SaveChangesAsync ( CancellationToken cancellationToken = default )
		=> await _context.SaveChangesAsync ( cancellationToken );

	public void Dispose ()
	{
		_context.Dispose ();
	}

	public async ValueTask DisposeAsync ()
	{
		await _context.DisposeAsync ();
	}
}