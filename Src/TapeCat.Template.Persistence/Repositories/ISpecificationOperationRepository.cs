namespace TapeCat.Template.Persistence.Repositories;

using Domain.Core.Models;
using Pagination;
using Specifications;

public interface ISpecificationOperationRepository<TModel, TKey>
	where TModel : class, IModel<TKey>
{
	Task<List<TModel>> FilterByAsync ( QuerySpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TModel>> FilterByAsync ( PaginationQuerySpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<List<TMappable>> FilterByAsync<TMappable> ( QuerySpecification<TModel , TKey> queasySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TMappable>> FilterByAsync<TMappable> ( PaginationQuerySpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<TModel?> FindByAsync ( QuerySpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<TMappable?> FindByAsync<TMappable> ( QuerySpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );
}