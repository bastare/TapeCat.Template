namespace TapeCat.Template.Persistence.Specifications.Evaluator;

using Domain.Core.Models;

public static class QuerySpecificationEvaluator
{
	public static IQueryable<TModel> SpecifiedQuery<TModel, TKey> ( this IQueryable<TModel> inputQuery ,
																	QuerySpecification<TModel , TKey> specification )
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

		static void InvokeIncludes ( ref IQueryable<TModel> inputQuery , in QuerySpecification<TModel , TKey> specification )
		{
			inputQuery = specification.Includes
				!.Invoke ( inputQuery );
		}

		static void InvokeConditions ( ref IQueryable<TModel> inputQuery , in QuerySpecification<TModel , TKey> specification )
		{
			inputQuery = specification.Conditions
				!.Invoke ( inputQuery );
		}

		static void InvokeOrderBy ( ref IQueryable<TModel> inputQuery , in QuerySpecification<TModel , TKey> specification )
		{
			inputQuery = specification.OrderBy
				!.Invoke ( inputQuery );
		}
	}
}