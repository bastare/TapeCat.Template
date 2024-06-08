namespace TapeCat.Template.Infrastructure.Persistence.Specifications.Evaluator;

using Domain.Core.Models;
using Inline;

public static class QuerySpecificationEvaluator
{
	public static IQueryable SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> inputQuery ,
															InlinePaginationQuerySpecification<TModel , TKey> specification )
		where TModel : class, IModel<TKey>
	{
		NotNull ( inputQuery );
		NotNull ( specification );

		if ( specification.Conditions is not null )
			InvokeConditions ( ref inputQuery , in specification );

		if ( specification.OrderBy is not null )
			InvokeOrderBy ( ref inputQuery , in specification );

		return specification.Projection is not null
			? InvokeProjection ( inputQuery , specification )
			: inputQuery;

		static void InvokeConditions (
			ref IQueryable<TModel> inputQuery ,
			in InlinePaginationQuerySpecification<TModel , TKey> specification )
		{
			inputQuery = specification.Conditions
				!.Invoke ( inputQuery );
		}

		static void InvokeOrderBy (
			ref IQueryable<TModel> inputQuery ,
			in InlinePaginationQuerySpecification<TModel , TKey> specification )
		{
			inputQuery = specification.OrderBy
				!.Invoke ( inputQuery );
		}

		static IQueryable InvokeProjection ( IQueryable<TModel> inputQuery , in IProjectionQuerySpecification<TModel , TKey> specification )
			=> specification.Projection!.Invoke ( inputQuery );
	}

	public static IQueryable<TModel> SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> inputQuery ,
																	Specification<TModel , TKey> specification )
		where TModel : class, IModel<TKey>
	{
		NotNull ( inputQuery );
		NotNull ( specification );

		if ( specification.Includes is not null )
			InvokeIncludes ( ref inputQuery , in specification );

		if ( specification.Conditions is not null )
			InvokeConditions ( ref inputQuery , in specification );

		if ( specification.OrderBy is not null )
			InvokeOrderBy ( ref inputQuery , in specification );

		return inputQuery;

		static void InvokeIncludes (
			ref IQueryable<TModel> inputQuery ,
			in Specification<TModel , TKey> specification )
		{
			inputQuery = specification.Includes
				!.Invoke ( inputQuery );
		}

		static void InvokeConditions (
			ref IQueryable<TModel> inputQuery ,
			in Specification<TModel , TKey> specification )
		{
			inputQuery = specification.Conditions
				!.Invoke ( inputQuery );
		}

		static void InvokeOrderBy (
			ref IQueryable<TModel> inputQuery ,
			in Specification<TModel , TKey> specification )
		{
			inputQuery = specification.OrderBy
				!.Invoke ( inputQuery );
		}
	}
}