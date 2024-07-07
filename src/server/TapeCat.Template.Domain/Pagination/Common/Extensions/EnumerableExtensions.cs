namespace TapeCat.Template.Domain.Pagination.Common.Extensions;

using Pagination;

public static class EnumerableExtensions
{
	public static PagedList<T> ToPagedList<T> ( this IEnumerable<T> collection , long count , long offset , long limit )
		=> PagedList<T>.Create ( collection , count , offset , limit );
}