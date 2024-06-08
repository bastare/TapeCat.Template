namespace TapeCat.Template.Infrastructure.Persistence.Specifications;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore.Query;

public abstract record Specification<TModel, TKey> :
	IQuerySpecification<TModel , TKey>,
	IJoinSpecification<TModel , TKey>
		where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? Conditions { get; protected init; }

	public Func<IQueryable<TModel> , IOrderedQueryable<TModel>>? OrderBy { get; protected init; }

	public Func<IQueryable<TModel> , IIncludableQueryable<TModel , object>>? Includes { get; protected init; }
}