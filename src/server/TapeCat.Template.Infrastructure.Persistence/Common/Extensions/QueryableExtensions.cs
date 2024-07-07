namespace TapeCat.Template.Infrastructure.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Pagination;
using System.Linq;
using System.Linq.Dynamic.Core;

public static class QueryableExtensions
{
	public async static Task<PagedList<T>> ToPagedListAsync<T> ( this IQueryable<T> queryable , ulong offset , ulong limit , CancellationToken cancellationToken = default )
		where T : class
	{
		var count = await GetCountOfTableRecordsAsync ( queryable , cancellationToken );

		var pagedData =
			await GetPagedRecords ( queryable , ( int ) offset , ( int ) limit )
				.ToListAsync ( cancellationToken );

		return PagedList<T>.Create ( pagedData , ( ulong ) count , offset , limit );

		static Task<long> GetCountOfTableRecordsAsync ( IQueryable<T> queryable , CancellationToken cancellationToken = default )
			=> queryable.LongCountAsync ( cancellationToken );
	}

	public async static Task<PagedList<object>> ToPagedListAsync (
		this IQueryable queryable ,
		ulong offset ,
		ulong limit ,
		CancellationToken cancellationToken = default )
	{
		var count = GetCountOfTableRecords ( queryable );

		var pagedData =
			await GetPagedRecords ( queryable , ( int ) offset , ( int ) limit )
				.ToDynamicListAsync ( cancellationToken );

		return PagedList<object>.Create ( pagedData , ( ulong ) count , offset , limit );

		static long GetCountOfTableRecords ( IQueryable queryable )
			=> queryable.LongCount ();
	}

	private static IQueryable GetPagedRecords ( IQueryable queryable , int offset , int limit )
		=> offset switch
		{
			>= 1 =>
				queryable
					.Skip ( ( offset - 1 ) * limit )
					.Take ( limit ),

			<= 0 =>
				throw new ArgumentException ( "Offset can`t be a negative number" , nameof ( offset ) )
		};

	private static IQueryable<T> GetPagedRecords<T> ( IQueryable<T> queryable , int offset , int limit )
		where T : class
			=> ( IQueryable<T> ) GetPagedRecords ( ( IQueryable ) queryable , offset , limit );
}