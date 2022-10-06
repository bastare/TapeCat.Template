namespace TapeCat.Template.Persistence.Common.Extensions;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pagination;

public static class MongoQueryableExtensions
{
	public async static Task<PagedList<T>> ToPagedListAsync<T> ( this IMongoQueryable<T> queryable , int offset , int limit , CancellationToken cancellationToken = default )
	{
		var count = await GetCountOfTableRecordsAsync ( queryable , cancellationToken );

		var pagedData =
			await GetPagedRecords ( queryable , offset , limit )
				.ToListAsync ( cancellationToken );

		return PagedList<T>.Create ( pagedData , count , offset , limit );

		static async Task<int> GetCountOfTableRecordsAsync ( IMongoQueryable<T> queryable , CancellationToken cancellationToken = default )
			=> await queryable.CountAsync ( cancellationToken );

		static IMongoQueryable<T> GetPagedRecords ( IMongoQueryable<T> queryable , int offset , int limit )
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
}