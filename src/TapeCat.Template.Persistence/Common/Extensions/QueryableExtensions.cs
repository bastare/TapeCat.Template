namespace TapeCat.Template.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq;
using System.Linq.Dynamic.Core;

public static class QueryableExtensions
{
	public async static Task<PagedList<T>> ToPagedListAsync<T> ( this IQueryable<T> queryable , int offset , int limit , CancellationToken cancellationToken = default )
	{
		var count = await GetCountOfTableRecordsAsync ( queryable , cancellationToken );

		var pagedData =
			await GetPagedRecords ( queryable , offset , limit )
				.ToListAsync ( cancellationToken );

		return PagedList<T>.Create ( pagedData , count , offset , limit );

		static async Task<int> GetCountOfTableRecordsAsync ( IQueryable<T> queryable , CancellationToken cancellationToken = default )
			=> await queryable.CountAsync ( cancellationToken );

		static IQueryable<T> GetPagedRecords ( IQueryable<T> queryable , int offset , int limit )
			 => offset switch
			 {
				 >= 1 =>
					 queryable
						 .Skip ( ( offset - 1 ) * limit )
						 .Take ( limit ),

				 <= 0 =>
					 throw new ArgumentException ( "Offset can`t be a negative number" , nameof ( offset ) )
			 };
	}

	public static IQueryable<TModel> OptionalWhere<TModel> ( this IQueryable<TModel> query , string? expression = default )
	{
		NotNull ( query , nameof ( query ) );

		return expression is null
			? query
			: query.Where ( expression );
	}

	public static IOrderedQueryable<TModel> OrderBy<TModel> ( this IQueryable<TModel> query , string orderableField , bool isDescending )
	{
		NotNull ( query , nameof ( query ) );
		NotNullOrEmpty ( orderableField , nameof ( orderableField ) );

		return query.OrderBy (
			ordering: CreateOrderableExpression ( orderableField , isDescending ) );

		static string CreateOrderableExpression ( string orderableField , bool isDescending )
			=> string.Concat ( orderableField , " " , str2: ResolveStringLiteralFromOrder ( isDescending ) );

		static string ResolveStringLiteralFromOrder ( bool isDescending )
			=> isDescending ? "desc" : "asc";
	}
}