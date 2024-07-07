namespace TapeCat.Template.Domain.Shared.Common.Extensions;

public static class EnumerableExtensions
{
	public static bool IsNullOrEmpty ( this IEnumerable? collection )
		=> !( collection?.Cast<object> ()
			.Any () ??
				default );

	public static bool ContainType<TExpectableType> ( this IEnumerable? collection )
		=> collection?.Cast<object> ()
			.Any ( element => element is TExpectableType ) ??
				throw new ArgumentNullException ( nameof ( collection ) , "Collection is null" );

	public static Task<T[]> WhenAllAsync<T> ( this IEnumerable<T> collection , Func<T , Task<T>> selector )
		=> Task.WhenAll (
			tasks: collection.Select ( selector ) );

	public static Task<T[]> WhenAllAsync<T> ( this IEnumerable<Task<T>> collection )
		=> Task.WhenAll ( collection );

	public static Task WhenAllAsync<T> ( this IEnumerable<T> collection , Func<T , Task> selector )
		=> Task.WhenAll (
			tasks: collection.Select ( selector ) );

	public static Task WhenAllAsync<T> ( this IEnumerable<Task> collection )
		=> Task.WhenAll ( collection );

	public static async IAsyncEnumerable<T> ToAsyncEnumerable<T> ( this IEnumerable<Task<T>> collection )
	{
		foreach ( var item in collection )
			yield return await item;
	}
}