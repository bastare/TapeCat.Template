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
	}

	private static void InvokeIncludes<TModel, TKey> ( ref IQueryable<TModel> inputQuery , in QuerySpecification<TModel , TKey> specification )
		where TModel : class, IModel<TKey>
	{
		inputQuery = specification.Includes
			!.Invoke ( inputQuery );
	}

	private static void InvokeConditions<TModel, TKey> ( ref IQueryable<TModel> inputQuery , in QuerySpecification<TModel , TKey> specification )
		where TModel : class, IModel<TKey>
	{
		inputQuery = specification.Conditions
			!.Invoke ( inputQuery );
	}

	private static void InvokeOrderBy<TModel, TKey> ( ref IQueryable<TModel> inputQuery , in QuerySpecification<TModel , TKey> specification )
		where TModel : class, IModel<TKey>
	{
		inputQuery = specification.OrderBy
			!.Invoke ( inputQuery );
	}
}