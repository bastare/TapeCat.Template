namespace TapeCat.Template.Persistence.Uow.Repositories.Interfaces;

using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

public interface IBulkOperationRepository<TModel, TKey>
	where TModel : class, IModel<TKey>
{
	Task BulkAddAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default );

	Task BulkRemoveByAsync ( Expression<Func<TModel , bool>> predicate , CancellationToken cancellationToken = default );

	Task BulkRemoveAsync ( IEnumerable<TModel> models , CancellationToken cancellationToken = default );

	void BulkUpdate ( IEnumerable<TModel> models );
}