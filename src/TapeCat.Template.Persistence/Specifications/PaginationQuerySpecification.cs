namespace TapeCat.Template.Persistence.Specifications;

using Domain.Core.Models;

public abstract record PaginationQuerySpecification<TModel, TKey> : QuerySpecification<TModel , TKey>
	where TModel : IModel<TKey>
{
	public int Limit { get; protected init; } = 1;

	public int Offset { get; protected init; } = 100;
}