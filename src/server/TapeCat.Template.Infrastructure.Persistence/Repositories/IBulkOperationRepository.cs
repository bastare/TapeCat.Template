namespace TapeCat.Template.Infrastructure.Persistence.Repositories;

using Domain.Core.Models;

public interface IBulkOperationRepository<TModel, TKey>
	where TModel : class, IModel<TKey>
{
	Task BulkAddAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default );

	Task BulkRemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task BulkRemoveAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default );

	Task BulkUpdateAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default );
}