namespace TapeCat.Template.Infrastructure.Persistence.Specifications;

using Domain.Core.Models;

public interface IQuerySpecification<TModel, TKey>
	where TModel : IModel<TKey>
{
	Func<IQueryable<TModel> , IQueryable<TModel>>? Conditions { get; }

	Func<IQueryable<TModel> , IOrderedQueryable<TModel>>? OrderBy { get; }
}