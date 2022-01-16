namespace TapeCat.Template.Persistence.Pagination;

using System;
using System.Collections.Generic;
using static Domain.Shared.Helpers.AssertGuard.Guard;

public sealed class PagedList<T> : List<T>
{
	public int CurrentOffset { get; private set; }

	public int TotalPages { get; private set; }

	public int Limit { get; private set; }

	public int TotalCount { get; private set; }

	private PagedList ( IEnumerable<T> items )
		: base ( items )
	{ }

	public static PagedList<T> Create ( IEnumerable<T> items , int count , int offset , int limit )
	{
		NotNull ( items , nameof ( items ) );

		var totalPages = CalculateTotalPages ( count , limit );

		return new ( items )
		{
			CurrentOffset = offset ,
			TotalPages = totalPages ,
			Limit = limit ,
			TotalCount = count
		};
	}

	private static int CalculateTotalPages ( int count , int limit )
		=> ( int ) Math.Ceiling ( count / ( double ) limit );
}