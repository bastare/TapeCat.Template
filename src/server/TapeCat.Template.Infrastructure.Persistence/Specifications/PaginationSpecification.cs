namespace TapeCat.Template.Infrastructure.Persistence.Specifications;

using Domain.Core.Models;

public abstract record PaginationSpecification<TModel, TKey> :
	Specification<TModel , TKey>,
	IPaginationQuerySpecification<TModel , TKey>
		where TModel : IModel<TKey>
{
	public int Limit { get; protected init; }

	public int Offset { get; protected init; }
}