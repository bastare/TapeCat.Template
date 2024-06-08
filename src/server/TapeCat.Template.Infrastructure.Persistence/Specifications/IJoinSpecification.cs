namespace TapeCat.Template.Infrastructure.Persistence.Specifications;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore.Query;

public interface IJoinSpecification<TModel, TKey>
	where TModel : IModel<TKey>
{
	Func<IQueryable<TModel> , IIncludableQueryable<TModel , object>>? Includes { get; }
}