namespace TapeCat.Template.Infrastructure.Persistence.Specifications.Inline;

using Domain.Core.Models;
using Domain.Contracts.Dtos.QueryDtos;
using DynamicLinqDecorator;
using Microsoft.EntityFrameworkCore.Query;
using System;

public sealed record InlinePaginationQuerySpecification<TModel, TKey> :
	IPaginationQuerySpecification<TModel , TKey>,
	IQuerySpecification<TModel , TKey>,
	IProjectionQuerySpecification<TModel , TKey>
		where TModel : IModel<TKey>
{
	public Func<IQueryable<TModel> , IQueryable<TModel>>? Conditions { get; init; }

	public Func<IQueryable<TModel> , IIncludableQueryable<TModel , object>>? Includes { get; init; }

	public Func<IQueryable<TModel> , IOrderedQueryable<TModel>>? OrderBy { get; init; }

	public Func<IQueryable , IQueryable>? Projection { get; init; }

	public int Limit { get; init; }

	public int Offset { get; init; }

	public InlinePaginationQuerySpecification (
		ExpressionQueryDto? expressionQuery = default ,
		OrderQueryDto? orderQuery = default ,
		PaginationQueryDto? paginationQuery = default ,
		ProjectionQueryDto? projectionQuery = default )
	{
		if ( HasExpression ( expressionQuery ) )
			Conditions = query =>
				query.Where ( expressionQuery );

		if ( HasOrdering ( orderQuery ) )
			OrderBy = query =>
				query.OrderBy ( orderQuery );

		if ( HasProjection ( projectionQuery ) )
			Projection = query =>
				query.Select ( projectionQuery );

		if ( HasPagination ( paginationQuery ) )
		{
			Offset = paginationQuery!.Offset;
			Limit = paginationQuery!.Limit;
		}

		static bool HasExpression ( ExpressionQueryDto? expressionQuery )
			=> !string.IsNullOrEmpty ( expressionQuery?.Expression );

		static bool HasOrdering ( OrderQueryDto? orderQuery )
			=> !string.IsNullOrEmpty ( orderQuery?.OrderBy )
				&& orderQuery?.IsDescending is not null;

		static bool HasProjection ( ProjectionQueryDto? projectionQuery )
			=> !string.IsNullOrEmpty ( projectionQuery?.Projection );

		static bool HasPagination ( PaginationQueryDto? paginationQuery )
			=> paginationQuery is not null;
	}
}