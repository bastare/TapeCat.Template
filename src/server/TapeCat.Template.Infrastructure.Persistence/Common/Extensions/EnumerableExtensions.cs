namespace TapeCat.Template.Infrastructure.Persistence.Common.Extensions;

using Pagination;

public static class EnumerableExtensions
{
	public static PagedList<T> ToPagedList<T> ( this IEnumerable<T> collection , int count , int offset , int limit )
		=> PagedList<T>.Create ( collection , count , offset , limit );
}