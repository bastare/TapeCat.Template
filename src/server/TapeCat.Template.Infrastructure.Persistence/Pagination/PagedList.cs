namespace TapeCat.Template.Infrastructure.Persistence.Pagination;

using Interfaces;

public sealed record PagedList<T> : IPagedList<T>
{
	private readonly IImmutableList<T> _immutablePagedList;

	public T? this[ int index ] => _immutablePagedList[ index ];

	public int CurrentOffset { get; private set; }

	public int TotalPages { get; private set; }

	public int Limit { get; private set; }

	public int TotalCount { get; private set; }

	public IEnumerator<T> GetEnumerator ()
		=> _immutablePagedList.GetEnumerator ();

	IEnumerator IEnumerable.GetEnumerator ()
		=> GetEnumerator ();

	private PagedList ( IEnumerable<T> items )
		=> _immutablePagedList = ImmutableList.CreateRange ( items );

	public static PagedList<T> Create ( IEnumerable<T> items , int count , int offset , int limit )
	{
		NotNull ( items );
		ParametersAreValid ( limit );

		return new ( items )
		{
			CurrentOffset = offset ,
			TotalPages = CalculateTotalPages ( count , limit ) ,
			Limit = limit ,
			TotalCount = count
		};

		static void ParametersAreValid ( int limit )
		{
			if ( limit <= 0 )
				throw new ArgumentException ( $"{nameof ( limit )}: {limit}, has the `zero` or negative value" );
		}

		static int CalculateTotalPages ( int count , int limit )
			=> ( int ) Math.Ceiling ( count / ( double ) limit );
	}
}