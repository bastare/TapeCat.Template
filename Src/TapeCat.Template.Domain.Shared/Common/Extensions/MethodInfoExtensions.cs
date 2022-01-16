namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Domain.Shared.Helpers.AssertGuard.Guard;

public static class MethodInfoExtensions
{
	public static void Invoke ( this MethodInfo methodInfo , Type typeOfInstance , params object[] parameters )
	{
		NotNull ( methodInfo , nameof ( methodInfo ) );
		NotNull ( typeOfInstance , nameof ( typeOfInstance ) );
		NotNullOrEmpty ( parameters , nameof ( parameters ) );

		if ( !methodInfo.HasParameters ( ResolveParametersFromPassingVariables ( parameters ) ) )
			throw new ArgumentNullException (
				paramName: methodInfo.Name ,
				message: CreateExceptionMessage ( parameters ) );

		methodInfo.Invoke (
			obj: Activator.CreateInstance ( typeOfInstance ) ,
			parameters: parameters );
	}

	public static void Invoke ( this MethodInfo methodInfo , Type typeOfInstance , IEnumerable parameters )
	{
		NotNull ( methodInfo , nameof ( methodInfo ) );
		NotNull ( typeOfInstance , nameof ( typeOfInstance ) );
		NotNullOrEmpty ( parameters , nameof ( parameters ) );

		if ( !methodInfo.HasParameters ( ResolveParametersFromPassingVariables ( parameters ) ) )
			throw new ArgumentNullException (
				paramName: methodInfo.Name ,
				message: CreateExceptionMessage ( in parameters ) );

		methodInfo.Invoke (
			obj: Activator.CreateInstance ( typeOfInstance ) ,
			parameters: parameters.Cast<object> ().ToArray () );
	}

	private static string CreateExceptionMessage ( in IEnumerable parameters )
		=> parameters.Cast<object> ()
			.Aggregate (
				new StringBuilder ( "Method doesn't accept this parameters (in current sequence):" ) ,
				( stringBuilder , parameter ) =>
					stringBuilder
						.Append ( parameter?.GetType () ?? typeof ( Nullable ) )
						.Append ( ' ' ) )

			.ToString ();

	public static bool HasParameters ( this MethodInfo methodInfo , params Type[] passingParametersTypes )
	{
		var methodParameters = methodInfo.ResolveParameters ();

		return HasAssignableParametersType ( methodParameters , passingParametersTypes )
			&& HasSameParametersQuantity ( methodParameters , passingParametersTypes );
	}

	public static bool HasParameters ( this MethodInfo methodInfo , IEnumerable<Type> passingParametersTypes )
	{
		var methodParameters = methodInfo.ResolveParameters ();

		return HasAssignableParametersType ( methodParameters , passingParametersTypes )
			&& HasSameParametersQuantity ( methodParameters , passingParametersTypes );
	}

	private static IEnumerable<Type> ResolveParameters ( this MethodInfo methodInfo )
		=> methodInfo.GetParameters ()
			.Select ( parameterInfo => parameterInfo.ParameterType );

	private static IEnumerable<Type> ResolveParametersFromPassingVariables ( IEnumerable passingVariables )
		=> passingVariables.Cast<object> ()
			.Select ( parameter => parameter?.GetType () ??
				typeof ( Nullable ) );

	private static bool HasSameParametersQuantity ( IEnumerable methodParameters , IEnumerable types )
		=> methodParameters.Cast<object> ().Count () == types.Cast<object> ().Count ();

	private static bool HasAssignableParametersType ( IEnumerable<Type> methodParametersTypes , IEnumerable<Type> passingParametersTypes )
	{
		var methodParametersEnumerator = methodParametersTypes.GetEnumerator ();

		var passingParametersTypesEnumerator = passingParametersTypes.GetEnumerator ();

		while ( methodParametersEnumerator.MoveNext () && passingParametersTypesEnumerator.MoveNext () )
		{
			var methodParameterType = methodParametersEnumerator.Current;

			var passingParameterType = passingParametersTypesEnumerator.Current;

			if ( !methodParameterType.IsAssignableFrom ( passingParameterType ) )
				return false;
		}

		return true;
	}
}