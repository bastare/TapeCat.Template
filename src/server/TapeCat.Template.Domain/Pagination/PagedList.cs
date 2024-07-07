namespace TapeCat.Template.Domain.Pagination;

using Shared.Interfaces;

public sealed record PagedList<T> : IPagedList<T>
{
	private readonly IReadOnlyList<T> _immutablePagedList;

	public T? this[ int index ] => _immutablePagedList[ index ];

	public long CurrentOffset { get; private set; }

	public long TotalPages { get; private set; }

	public long Limit { get; private set; }

	public long TotalCount { get; private set; }

	public IEnumerator<T> GetEnumerator ()
		=> _immutablePagedList.GetEnumerator ();

	IEnumerator IEnumerable.GetEnumerator ()
		=> GetEnumerator ();

	private PagedList ( IEnumerable<T> items )
		=> _immutablePagedList = [ .. items ];

	public static PagedList<T> Create ( IEnumerable<T> items , long count , long offset , long limit )
	{
		NotNull ( items );
		ParametersAreValid ( limit );

		return new ( items )
		{
			CurrentOffset = offset ,
			TotalPages = ( long ) CalculateTotalPages ( count , limit ) ,
			Limit = limit ,
			TotalCount = count
		};

		static void ParametersAreValid ( long limit )
		{
			if ( limit <= 0 )
				throw new ArgumentException ( $"{nameof ( limit )}: {limit}, has the `zero` or negative value" );
		}

		static double CalculateTotalPages ( long count , long limit )
			=> Math.Ceiling ( count / ( double ) limit );
	}
}