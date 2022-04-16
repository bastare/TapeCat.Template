namespace TapeCat.Template.Persistence.Repositories.Ef;

using Common.Exceptions;
using Common.Extensions;
using Domain.Core.Models;
using Domain.Shared.Common.Exceptions;
using Domain.Shared.Common.Extensions;
using EFCore.BulkExtensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Pagination;
using Specifications;

public sealed class EfRepository<TModel, TKey, TContext> :
	IRepository<TModel , TKey>,
	IBulkOperationRepository<TModel , TKey>,
	ISpecificationOperationRepository<TModel , TKey>,
	ITrackingRepository<TModel , TKey>
		where TModel : class, IModel<TKey>
		where TContext : DbContext
{
	private readonly TContext _context;

	private readonly TypeAdapterConfig _typeAdapterConfig;

	public EfRepository ( TContext context , TypeAdapterConfig typeAdapterConfig )
	{
		_context = context;
		_typeAdapterConfig = typeAdapterConfig;
	}

	public async Task<TModel> AddAsync ( TModel model , CancellationToken cancellationToken = default )
	{
		NotNull ( model );

		return ( await _context.Set<TModel> ()
			.AddAsync ( model , cancellationToken ) )

			.Entity;
	}

	public async Task<List<TModel>> GetAllAsync ( bool isTracking , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.ToListAsync ( cancellationToken );

	public async Task<List<TTransform>> GetAllAsync<TTransform> ( Expression<Func<TModel , TTransform>> selector ,
																  bool isTracking ,
																  CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.Select ( selector )

			.ToListAsync ( cancellationToken );

	public async Task<PagedList<TModel>> GetAllAsync ( int offset , int limit , bool isTracking , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.ToPagedListAsync ( offset , limit , cancellationToken );

	public async Task<List<TMappable>> GetAllAsync<TMappable> ( bool isTracking , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToListAsync ( cancellationToken );

	public async Task<PagedList<TMappable>> GetAllAsync<TMappable> ( int offset , int limit , bool isTracking , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToPagedListAsync ( offset , limit , cancellationToken );

	public async Task<List<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.Where ( predicate )

			.ToListAsync ( cancellationToken );
	}

	public async Task<PagedList<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , int offset , int limit , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.Where ( predicate )

			.ToPagedListAsync ( offset , limit , cancellationToken );
	}

	public async Task<List<TModel>> FilterByAsync ( QuerySpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ToListAsync ( cancellationToken );
	}

	public async Task<PagedList<TModel>> FilterByAsync ( PaginationQuerySpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ToPagedListAsync ( specification.Offset , specification.Limit , cancellationToken );
	}

	public async Task<List<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.Where ( predicate )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToListAsync ( cancellationToken );
	}

	public async Task<PagedList<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , int offset , int limit , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.Where ( predicate )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToPagedListAsync ( offset , limit , cancellationToken );
	}

	public async Task<List<TMappable>> FilterByAsync<TMappable> ( QuerySpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToListAsync ( cancellationToken );
	}

	public async Task<PagedList<TMappable>> FilterByAsync<TMappable> ( PaginationQuerySpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToPagedListAsync ( specification.Offset , specification.Limit , cancellationToken );
	}

	public async Task<TModel?> FindByAsync ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.SingleOrDefaultAsync ( predicate , cancellationToken );
	}

	public async Task<TMappable?> FindByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.Where ( predicate )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.SingleOrDefaultAsync ( cancellationToken );
	}

	public async Task<TModel?> FindByAsync ( QuerySpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.SingleOrDefaultAsync ( cancellationToken );
	}

	public async Task<TMappable?> FindByAsync<TMappable> ( QuerySpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.SingleOrDefaultAsync ( cancellationToken );
	}

	public async Task<TModel?> GetAsync ( TKey id , bool isTracking , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.SingleOrDefaultAsync ( model => model.Id!.Equals ( id ) , cancellationToken );

	public async Task<TMappable?> GetAsync<TMappable> ( TKey id , bool isTracking , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking )
			.Where ( model => model.Id!.Equals ( id ) )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.SingleOrDefaultAsync ( cancellationToken );

	public async Task<bool> IsExistAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ( isTracking: false )
			.AnyAsync ( predicate , cancellationToken );

	public async Task<TModel?> RemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
	{
		NotNull ( predicate );

		return ( await FindByAsync ( predicate , isTracking: true , cancellationToken ) )!

			.Tap ( entityToRemove =>
			 {
				 if ( entityToRemove is null )
					 throw new NotFoundException ( message: "No entity was founded according this expression" );

				 var entityState = _context.Set<TModel> ()
					.Remove ( entityToRemove );

				 if ( entityState is { State: not EntityState.Deleted } )
					 throw new RepositoryException ( "Entity wasn`t removed" );
			 } )!;
	}

	public Task<TModel?> RemoveAsync ( TModel model , CancellationToken cancellationToken = default )
		=> Task.FromResult (
			result: model.Tap ( model =>
			  {
				  NotNull ( model , nameof ( model ) );

				  var entityState = _context.Set<TModel> ()
				   .Remove ( model! );

				  if ( entityState is { State: not EntityState.Deleted } )
					  throw new RepositoryException ( "Entity wasn`t removed" );
			  } ) )!;

	public async Task RemoveRangeAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
	{
		NotNullOrEmpty ( models , nameof ( models ) );

		foreach ( var modelId
			in models.Select ( model => model.Id ) )
		{
			var entityToRemove =
				await GetAsync ( modelId , isTracking: true , cancellationToken ) ??
					throw new NotFoundException ( message: $"No entity with this id: {modelId}" );

			var entityState = _context.Set<TModel> ()
				.Remove ( entityToRemove );

			if ( entityState is { State: not EntityState.Deleted } )
				throw new RepositoryException ( "Entity wasn`t removed" );
		}
	}

	public async Task<List<TModel>> GetAllAsync ( CancellationToken cancellationToken = default )
		=> await GetAllAsync ( isTracking: false , cancellationToken );

	public async Task<List<TTransform>> GetAllAsync<TTransform> ( Expression<Func<TModel , TTransform>> selector , CancellationToken cancellationToken = default )
		=> await GetAllAsync ( selector , isTracking: false , cancellationToken );

	public async Task<PagedList<TModel>> GetAllAsync ( int offset , int limit , CancellationToken cancellationToken = default )
		=> await GetAllAsync ( offset , limit , isTracking: false , cancellationToken );

	public async Task<List<TMappable>> GetAllAsync<TMappable> ( CancellationToken cancellationToken = default )
		=> await GetAllAsync<TMappable> ( isTracking: false , cancellationToken );

	public async Task<PagedList<TMappable>> GetAllAsync<TMappable> ( int offset , int limit , CancellationToken cancellationToken = default )
		=> await GetAllAsync<TMappable> ( offset , limit , isTracking: false , cancellationToken );

	public async Task<List<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await FilterByAsync ( predicate , isTracking: false , cancellationToken );

	public async Task<PagedList<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default )
		=> await FilterByAsync ( predicate , offset , limit , isTracking: false , cancellationToken );

	public async Task<List<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await FilterByAsync<TMappable> ( predicate , isTracking: false , cancellationToken );

	public async Task<PagedList<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default )
		=> await FilterByAsync<TMappable> ( predicate , offset , limit , isTracking: false , cancellationToken );

	public async Task<TModel?> FindByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await FindByAsync ( predicate , isTracking: true , cancellationToken );

	public async Task<TMappable?> FindByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await FindByAsync<TMappable> ( predicate , isTracking: true , cancellationToken );

	public async Task<TModel?> GetAsync ( TKey id , CancellationToken cancellationToken = default )
		=> await GetAsync ( id , isTracking: true , cancellationToken );

	public async Task<TMappable?> GetAsync<TMappable> ( TKey id , CancellationToken cancellationToken = default )
		=> await GetAsync<TMappable> ( id , isTracking: true , cancellationToken );

	public Task<TModel> UpdateAsync ( TModel model , CancellationToken _ = default )
		=> Task.FromResult (
			result: model.Tap ( model =>
			  {
				  NotNull ( model , nameof ( model ) );

				  _context.Entry ( model! ).State = EntityState.Modified;

				  return _context.Entry ( model! ).Entity;
			  } ) )!;

	public async Task BulkAddAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
	{
		await _context.BulkInsertAsync (
			entities: models.ToList () ,
			cancellationToken: cancellationToken );
	}

	public async Task BulkRemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
	{
		var entitiesToRemove =
			await FilterByAsync ( predicate , isTracking: true , cancellationToken );

		await _context.BulkDeleteAsync (
			entities: entitiesToRemove ,
			cancellationToken: cancellationToken );
	}

	public async Task BulkRemoveAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
	{
		await _context.BulkDeleteAsync (
			entities: models.ToList () ,
			cancellationToken: cancellationToken );
	}

	public void BulkUpdate ( IEnumerable<TModel> models )
	{
		_context.BulkUpdate (
			entities: models.ToList () );
	}
}