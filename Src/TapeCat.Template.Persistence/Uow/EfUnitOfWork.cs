namespace TapeCat.Template.Persistence.Uow;

using Common.Exceptions;
using Domain.Core.Models;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public sealed class EfUnitOfWork<TContext, TKey> : IUnitOfWork<TKey>
	where TContext : DbContext
{
	private readonly TContext _context;

	private readonly TypeAdapterConfig _typeAdapterConfig;

	public EfUnitOfWork ( TContext context , TypeAdapterConfig typeAdapterConfig )
	{
		_context = context;
		_typeAdapterConfig = typeAdapterConfig;
	}

	public IRepository<TModel , TKey> Repository<TModel> ()
		where TModel : class, IModel<TKey>
			=> new EfRepository<TModel , TKey , TContext> ( _context , _typeAdapterConfig );

	public async Task CommitAsync ( CancellationToken cancellationToken = default )
	{
		if ( !await TryCommitAsync ( cancellationToken ) )
			throw new DataWasNotSavedException ();
	}

	public async Task<bool> TryCommitAsync ( CancellationToken cancellationToken = default ) =>
		await _context.SaveChangesAsync ( cancellationToken ) != 0;

	public void Dispose ()
	{
		_context.Dispose ();
	}

	public async ValueTask DisposeAsync ()
	{
		await _context.DisposeAsync ();
	}
}