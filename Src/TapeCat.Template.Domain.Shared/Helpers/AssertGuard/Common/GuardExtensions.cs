namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard.Common;

using System.Runtime.CompilerServices;
using TapeCat.Template.Domain.Shared.Common.Extensions;

public static class GuardExtensions
{
	public static TSelf NotNull<TSelf> ( this TSelf? self , [CallerArgumentExpression ( "self" )] string? variableName = default , string? message = default )
		where TSelf : class
			=> self!.Tap ( self => { Guard.NotNull ( self , variableName , message ); } )!;

	public static TSelf NotNull<TSelf, TException> ( this TSelf? self , [CallerArgumentExpression ( "self" )] string? variableName = default , string? message = default )
		where TSelf : class
		where TException : ArgumentNullException
			=> self!.Tap ( self => { Guard.NotNull<TException> ( self , variableName , message ); } )!;

	public static TSelf NotNullOrEmpty<TSelf> ( this TSelf? self , [CallerArgumentExpression ( "self" )] string? variableName = default , string? message = default )
		where TSelf : class, IEnumerable
			=> self!.Tap ( self => { Guard.NotNullOrEmpty ( self , variableName , message ); } )!;

	public static TSelf NotNullOrEmpty<TSelf, TException> ( this TSelf? self , [CallerArgumentExpression ( "self" )] string? variableName = default , string? message = default )
		where TSelf : class, IEnumerable
		where TException : ArgumentNullException
			=> self!.Tap ( self => { Guard.NotNullOrEmpty<TException> ( self , variableName , message ); } )!;
}