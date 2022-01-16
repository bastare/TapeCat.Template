namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using Humanizer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class StringExtensions
{
	public static string ToPluralForm ( this string @string )
		=> @string?.Pluralize ( inputIsKnownToBeSingular: false ) ??
			throw new ArgumentNullException ( nameof ( @string ) , "String is null" );

	public static string ToSingalForm ( this string @string )
		=> @string?.Singularize ( inputIsKnownToBePlural: false ) ??
			throw new ArgumentNullException ( nameof ( @string ) , "String is null" );

	public static byte[] ToBase64 ( this string @string )
		=> Convert.FromBase64String ( @string );

	public static string ToSHA256 ( this string @string )
	{
		var hashedCacheKey = GetHash ( @string );

		return hashedCacheKey
			.Aggregate (
				new StringBuilder () ,
				( stringBuilder , hashByte ) =>
					stringBuilder.Append ( hashByte.ToString ( "X2" ) ) )

			.ToString ();

		static byte[] GetHash ( string inputString )
			=> SHA256.Create ()
				.ComputeHash ( Encoding.UTF8.GetBytes ( inputString ) );
	}
}