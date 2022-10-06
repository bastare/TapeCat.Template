namespace TapeCat.Template.Persistence.Specifications;

using Domain.Core.Models;

public interface IProjectionQuerySpecification<TModel, TKey>
	where TModel : IModel<TKey>
{
	Func<IQueryable , IQueryable>? Projection { get; }
}