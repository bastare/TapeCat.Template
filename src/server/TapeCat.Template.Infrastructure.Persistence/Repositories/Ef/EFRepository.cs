namespace TapeCat.Template.Infrastructure.Persistence.Repositories.Ef;

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
using Specifications.Evaluator;
using Specifications.Inline;
using System.Linq;

public sealed class EfRepository<TModel, TKey, TContext> ( TContext context , TypeAdapterConfig typeAdapterConfig ) :
	IRepository<TModel , TKey>,
	IBulkOperationRepository<TModel , TKey>,
	ISpecificationOperationRepository<TModel , TKey>,
	ITrackingRepository<TModel , TKey>
		where TModel : class, IModel<TKey>
		where TContext : DbContext
{
	private readonly TContext _context = context;

	private readonly TypeAdapterConfig _typeAdapterConfig = typeAdapterConfig;

	public IQueryable<TModel> Query => _context.Set<TModel> ();

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
		where TMappable : class
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

	public async Task<List<TModel>> FilterByAsync ( Specification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ToListAsync ( cancellationToken );
	}

	public async Task<PagedList<TModel>> FilterByAsync ( PaginationSpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ToPagedListAsync ( specification.Offset , specification.Limit , cancellationToken );
	}

	public async Task<PagedList<object>> FilterByAsync ( InlinePaginationQuerySpecification<TModel , TKey> specification )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking: false )
			.SpecifiedQuery ( specification )

			.ToPagedListAsync ( specification.Offset , specification.Limit );
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
		where TMappable : class
	{
		NotNull ( predicate );

		return await _context.Set<TModel> ( isTracking )
			.Where ( predicate )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToPagedListAsync ( offset , limit , cancellationToken );
	}

	public async Task<List<TMappable>> FilterByAsync<TMappable> ( Specification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.ProjectToType<TMappable> ( _typeAdapterConfig )

			.ToListAsync ( cancellationToken );
	}

	public async Task<PagedList<TMappable>> FilterByAsync<TMappable> ( PaginationSpecification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
		where TMappable : class
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

	public async Task<TModel?> FindByAsync ( Specification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
	{
		NotNull ( specification );

		return await _context.Set<TModel> ( isTracking )
			.SpecifiedQuery ( specification )

			.SingleOrDefaultAsync ( cancellationToken );
	}

	public async Task<TMappable?> FindByAsync<TMappable> ( Specification<TModel , TKey> specification , bool isTracking , CancellationToken cancellationToken = default )
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
					  return;

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

	public Task RemoveRangeAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
	{
		NotNullOrEmpty ( models , nameof ( models ) );

		var modelsIds_ = ResolveIds ( models );

		return _context.Set<TModel> ()
			.Where ( model =>
				modelsIds_.Any ( id => id!.Equals ( model.Id ) ) )

			.ExecuteDeleteAsync ( cancellationToken );

		static IEnumerable<TKey> ResolveIds ( IEnumerable<TModel> models )
			=> models.Select ( model => model.Id );
	}

	public Task<List<TModel>> GetAllAsync ( CancellationToken cancellationToken = default )
		=> GetAllAsync ( isTracking: false , cancellationToken );

	public Task<List<TTransform>> GetAllAsync<TTransform> ( Expression<Func<TModel , TTransform>> selector , CancellationToken cancellationToken = default )
		=> GetAllAsync ( selector , isTracking: false , cancellationToken );

	public Task<PagedList<TModel>> GetAllAsync ( int offset , int limit , CancellationToken cancellationToken = default )
		=> GetAllAsync ( offset , limit , isTracking: false , cancellationToken );

	public Task<List<TMappable>> GetAllAsync<TMappable> ( CancellationToken cancellationToken = default )
		where TMappable : class
			=> GetAllAsync<TMappable> ( isTracking: false , cancellationToken );

	public Task<PagedList<TMappable>> GetAllAsync<TMappable> ( int offset , int limit , CancellationToken cancellationToken = default )
		where TMappable : class
			=> GetAllAsync<TMappable> ( offset , limit , isTracking: false , cancellationToken );

	public Task<List<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> FilterByAsync ( predicate , isTracking: false , cancellationToken );

	public Task<PagedList<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default )
		=> FilterByAsync ( predicate , offset , limit , isTracking: false , cancellationToken );

	public Task<List<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		where TMappable : class
			=> FilterByAsync<TMappable> ( predicate , isTracking: false , cancellationToken );

	public Task<PagedList<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default )
		where TMappable : class
			=> FilterByAsync<TMappable> ( predicate , offset , limit , isTracking: false , cancellationToken );

	public Task<TModel?> FindByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> FindByAsync ( predicate , isTracking: true , cancellationToken );

	public Task<TMappable?> FindByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		where TMappable : class
			=> FindByAsync<TMappable> ( predicate , isTracking: true , cancellationToken );

	public Task<TModel?> GetAsync ( TKey id , CancellationToken cancellationToken = default )
		=> GetAsync ( id , isTracking: true , cancellationToken );

	public Task<TMappable?> GetAsync<TMappable> ( TKey id , CancellationToken cancellationToken = default )
		=> GetAsync<TMappable> ( id , isTracking: true , cancellationToken );

	public Task<TModel> UpdateAsync ( TModel model , CancellationToken _ = default )
		=> Task.FromResult (
			result: model.Tap ( model =>
			  {
				  NotNull ( model );

				  _context.Entry ( model! ).State = EntityState.Modified;

				  return _context.Entry ( model! ).Entity;
			  } ) )!;

	public Task BulkAddAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
		=> _context.BulkInsertAsync (
			entities: models.ToList () ,
			cancellationToken: cancellationToken );

	public async Task BulkRemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
	{
		var entitiesToRemove =
			await FilterByAsync ( predicate , isTracking: true , cancellationToken );

		await _context.BulkDeleteAsync (
			entities: entitiesToRemove ,
			cancellationToken: cancellationToken );
	}

	public Task BulkRemoveAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
		=> _context.BulkDeleteAsync (
			entities: models.ToList () ,
			cancellationToken: cancellationToken );

	public Task BulkUpdateAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default )
		=> _context.BulkUpdateAsync (
			entities: models.ToList () ,
			cancellationToken: cancellationToken );

	public async Task<List<TModel>> ProjectionUpdateAsync<TSource> (
		TSource source ,
		Expression<Func<TModel , bool>> predicate ,
		Action<TModel , TSource> projectionAction ,
		CancellationToken cancellationToken = default )
			=> ( await FilterByAsync ( predicate , isTracking: true , cancellationToken ) )
				.Tap ( self =>
				  {
					  self.ForEach ( model => projectionAction ( model , source ) );
				  } );

	public async Task<List<TModel>> ProjectionUpdateAsync<TSource> (
		IEnumerable<TSource> source ,
		Expression<Func<TModel , bool>> predicate ,
		Action<TModel , IEnumerable<TSource>> projectionAction ,
		CancellationToken cancellationToken = default )
			=> ( await FilterByAsync ( predicate , isTracking: true , cancellationToken ) )
				.Tap ( self =>
				  {
					  self.ForEach ( model => projectionAction ( model , source ) );
				  } );

	public async Task<long> CountAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default )
		=> await _context.Set<TModel> ()
			.CountAsync ( predicate , cancellationToken );

	public long EffectedRowsAmount ( EntityState entityState )
		=> _context.ChangeTracker.Entries<TModel> ()
			.Count ( entry => entry.State == entityState );

	public ImmutableList<TModel> GetTrackingEntities ( EntityState entityState )
		=> _context.ChangeTracker.Entries<TModel> ()
			.Where ( entry => entry.State == entityState )
			.Select ( entry => entry.Entity )
			.ToImmutableList ();
}