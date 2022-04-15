namespace TapeCat.Template.Persistence.Specifications;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;

public abstract record QuerySpecification<TModel, TKey>
	where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? Conditions { get; protected init; }

	public Func<IQueryable<TModel> , IIncludableQueryable<TModel , object>>? Includes { get; protected init; }

	public Func<IQueryable<TModel> , IOrderedQueryable<TModel>>? OrderBy { get; protected init; }
}