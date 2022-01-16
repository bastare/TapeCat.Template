namespace TapeCat.Template.Persistence.Pagination.Common.Extensions;

using Pagination;
using System;
using System.Linq;
using static Domain.Shared.Helpers.AssertGuard.Guard;

public static class PagedListExtensions
{
	public static PagedList<TTransformResult> Select<T, TTransformResult> ( this PagedList<T> pagedList , Func<T , TTransformResult> selector )
	{
		NotNullOrEmpty ( pagedList , nameof ( pagedList ) );
		NotNull ( selector , nameof ( selector ) );

		var transformedCollection =
			Enumerable.Select ( pagedList , selector );

		return PagedList<TTransformResult>.Create (
			transformedCollection ,
			pagedList.TotalCount ,
			pagedList.CurrentOffset ,
			pagedList.Limit );
	}
}