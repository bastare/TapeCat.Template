namespace TapeCat.Template.Persistence.Repositories;

using Domain.Core.Models;
using Pagination;

public interface IRepository<TModel, TKey>
	where TModel : class, IModel<TKey>
{
	Task<TModel> AddAsync ( TModel model , CancellationToken cancellationToken = default );

	Task<List<TModel>> GetAllAsync ( CancellationToken cancellationToken = default );

	Task<List<TTransform>> GetAllAsync<TTransform> ( Expression<Func<TModel , TTransform>> selector , CancellationToken cancellationToken = default );

	Task<PagedList<TModel>> GetAllAsync ( int offset , int limit , CancellationToken cancellationToken = default );

	Task<List<TMappable>> GetAllAsync<TMappable> ( CancellationToken cancellationToken = default );

	Task<PagedList<TMappable>> GetAllAsync<TMappable> ( int offset , int limit , CancellationToken cancellationToken = default );

	Task<List<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task<PagedList<TModel>> FilterByAsync ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default );

	Task<List<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task<PagedList<TMappable>> FilterByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , int offset , int limit , CancellationToken cancellationToken = default );

	Task<TModel?> FindByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task<TMappable?> FindByAsync<TMappable> ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task<TModel?> GetAsync ( TKey id , CancellationToken cancellationToken = default );

	Task<TMappable?> GetAsync<TMappable> ( TKey id , CancellationToken cancellationToken = default );

	Task<bool> IsExistAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task<TModel?> RemoveAsync ( TModel model , CancellationToken cancellationToken = default );

	Task<TModel?> RemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task<TModel> UpdateAsync ( TModel model , CancellationToken cancellationToken = default );
}