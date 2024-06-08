namespace TapeCat.Template.Infrastructure.Persistence.Repositories;

using Domain.Core.Models;
using Pagination;
using Specifications;

public interface ISpecificationOperationRepository<TModel, TKey>
	where TModel : class, IModel<TKey>
{
	Task<List<TModel>> FilterByAsync ( Specification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TModel>> FilterByAsync ( PaginationSpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<List<TMappable>> FilterByAsync<TMappable> ( Specification<TModel , TKey> queasySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<PagedList<TMappable>> FilterByAsync<TMappable> ( PaginationSpecification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default )
		where TMappable : class;

	Task<TModel?> FindByAsync ( Specification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );

	Task<TMappable?> FindByAsync<TMappable> ( Specification<TModel , TKey> querySpecification , bool isTracking , CancellationToken cancellationToken = default );
}