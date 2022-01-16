namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class EnumerableExtensions
{
	public static bool IsNullOrEmpty ( this IEnumerable? collection )
		=> !( collection?.Cast<object> ()
			.Any () ??
				default );

	public static bool ContainType<TContain> ( this IEnumerable? collection )
		=> !( collection?.OfType<TContain> ()
			.IsNullOrEmpty () ??
				throw new ArgumentNullException ( nameof ( collection ) , "Collection is null" ) );

	public static async Task<IEnumerable<T>> WhenAllAsync<T> ( this IEnumerable<Task<T>> collection )
		=> await Task.WhenAll ( collection );
}