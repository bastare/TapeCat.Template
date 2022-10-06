namespace TapeCat.Template.Persistence.Specifications.DynamicLinqDecorator;

using Contracts.Dtos.QueryDtos;
using System.Linq;
using System.Linq.Dynamic.Core;

public static class DynamicLinqDecoratorExtensions
{
	public static IQueryable<TModel> Where<TModel> ( this IQueryable<TModel> query , ExpressionQueryDto? expressionQuery )
	{
		NotNull ( query );
		NotNull ( expressionQuery );

		try
		{
			return query.Where ( expressionQuery!.Expression! );
		}
		catch ( Exception exception )
		{
			throw new LinqSyntaxException (
				message: string.Concat ( "Expression: " , exception.Message ) ,
				innerException: exception );
		}
	}

	public static IOrderedQueryable<TModel> OrderBy<TModel> ( this IQueryable<TModel> query , OrderQueryDto? orderQuery )
	{
		NotNull ( query );
		NotNull ( orderQuery );

		try
		{
			return query.OrderBy ( orderQuery!.OrderBy! , orderQuery.IsDescending );
		}
		catch ( Exception exception )
		{
			throw new LinqSyntaxException (
				message: string.Concat ( "Order: " , exception.Message ) ,
				innerException: exception );
		}
	}

	public static IQueryable Select ( this IQueryable query , ProjectionQueryDto? projectionQuery )
	{
		NotNull ( query );
		NotNull ( projectionQuery );

		try
		{
			return query.Select ( projectionQuery!.Projection! );
		}
		catch ( Exception exception )
		{
			throw new LinqSyntaxException (
				message: string.Concat ( "Projection: " , exception.Message ) ,
				innerException: exception );
		}
	}
}