namespace TapeCat.Template.Infrastructure.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq;
using System.Linq.Dynamic.Core;

public static class QueryableExtensions
{
	public async static Task<PagedList<T>> ToPagedListAsync<T> ( this IQueryable<T> queryable , int offset , int limit , CancellationToken cancellationToken = default )
		where T : class
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

	public async static Task<PagedList<object>> ToPagedListAsync (
		this IQueryable queryable ,
		int offset ,
		int limit )
	{
		var count = GetCountOfTableRecords ( queryable );

		var pagedData =
			await GetPagedRecords ( queryable , offset , limit )
				.ToDynamicListAsync ();

		return PagedList<object>.Create ( pagedData , count , offset , limit );

		static int GetCountOfTableRecords ( IQueryable queryable )
			=> queryable.Count ();

		static IQueryable GetPagedRecords ( IQueryable queryable , int offset , int limit )
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

	public static IOrderedQueryable<TModel> OrderBy<TModel> ( this IQueryable<TModel> query , string orderableField , bool isDescending )
	{
		NotNull ( query );
		NotNullOrEmpty ( orderableField );

		return query.OrderBy (
			ordering: CreateOrderableExpression ( orderableField , isDescending ) );

		static string CreateOrderableExpression ( string orderableField , bool isDescending )
			=> string.Concat ( orderableField , " " , str2: ResolveStringLiteralFromOrder ( isDescending ) );

		static string ResolveStringLiteralFromOrder ( bool isDescending )
			=> isDescending ? "desc" : "asc";
	}
}