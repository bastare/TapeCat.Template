namespace TapeCat.Template.Persistence.Common.Extensions
{
	using Pagination;
	using System.Collections.Generic;

	public static class EnumerableExtensions
	{
		public static PagedList<T> ToPagedList<T> ( this IEnumerable<T> collection , int count , int offset , int limit )
			=> PagedList<T>.Create ( collection , count , offset , limit );
	}
}