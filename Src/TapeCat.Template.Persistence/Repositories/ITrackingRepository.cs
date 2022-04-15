namespace TapeCat.Template.Persistence.Repositories;

using Domain.Core.Models;
using Pagination;

public interface ITrackingRepository<TModel, TKey>
	where TModel : class, IModel<TKey>
{
	Task<List<TModel>> GetAllAsync ( bool isTracking , CancellationToken cancellationToken = default );

	Task<List<TTransform>> GetAllAsync<TTransform> ( Expression<Func<TModel , TTransform>> selector , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TModel>> GetAllAsync ( int offset , int limit , bool isTracking , CancellationToken cancellationToken = default );

	Task<List<TMappable>> GetAllAsync<TMappable> ( bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TMappable>> GetAllAsync<TMappable> ( int offset , int limit , bool isTracking , CancellationToken cancellationToken = default );

	Task<List<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , int offset , int limit , bool isTracking , CancellationToken cancellationToken = default );

	Task<List<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , int offset , int limit , bool isTracking , CancellationToken cancellationToken = default );

	Task<TModel?> FindByAsync ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default );

	Task<TMappable?> FindByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , bool isTracking , CancellationToken cancellationToken = default );

	Task<TModel?> GetAsync ( TKey id , bool isTracking , CancellationToken cancellationToken = default );

	Task<TMappable?> GetAsync<TMappable> ( TKey id , bool isTracking , CancellationToken cancellationToken = default );
}