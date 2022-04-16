namespace TapeCat.Template.Domain.Shared.Common.Extensions;

public static class MethodInfoExtensions
{
	public static void Invoke ( this MethodInfo methodInfo , Type typeOfInstance , params object[] parameters )
	{
		SafeInvoke ( methodInfo , typeOfInstance , parameters.AsEnumerable () );
	}

	public static void Invoke<T> ( this MethodInfo methodInfo , params object[] parameters )
	{
		SafeInvoke ( methodInfo , typeof ( T ) , parameters.AsEnumerable () );
	}

	public static void Invoke ( this MethodInfo methodInfo , Type typeOfInstance , IEnumerable parameters )
	{
		SafeInvoke ( methodInfo , typeOfInstance , parameters );
	}

	public static void Invoke<T> ( this MethodInfo methodInfo , IEnumerable parameters )
	{
		SafeInvoke ( methodInfo , typeof ( T ) , parameters );
	}

	private static void SafeInvoke ( MethodInfo methodInfo , Type typeOfInstance , IEnumerable parameters )
	{
		NotNull ( methodInfo );
		NotNull ( typeOfInstance );
		NotNullOrEmpty ( parameters );

		if ( !methodInfo.HasParameters ( passingParametersTypes: ResolveParametersFromPassingVariables ( parameters ) ) )
			throw new ArgumentNullException (
				paramName: methodInfo.Name ,
				message: CreateExceptionMessage ( in parameters ) );

		methodInfo.Invoke (
			obj: Activator.CreateInstance ( typeOfInstance ) ,
			parameters: parameters.Cast<object> ().ToArray () );

		static IEnumerable<Type> ResolveParametersFromPassingVariables ( IEnumerable passingVariables )
			=> passingVariables.Cast<object> ()
				.Select ( parameter => parameter?.GetType () ??
					typeof ( Nullable ) );

		static string CreateExceptionMessage ( in IEnumerable parameters )
			=> parameters.Cast<object> ()
				.Aggregate (
					new StringBuilder ( "Method doesn't accept this parameters (in current sequence):" ) ,
					( stringBuilder , parameter ) =>
						stringBuilder
							.Append ( parameter?.GetType () ?? typeof ( Nullable ) )
							.Append ( ' ' ) )

				.ToString ();
	}

	public static bool HasParameters ( this MethodInfo methodInfo , params Type[] passingParametersTypes )
		=> HasParameters ( methodInfo , passingParametersTypes.AsEnumerable () );

	public static bool HasParameters ( this MethodInfo methodInfo , IEnumerable<Type> passingParametersTypes )
	{
		var methodParameters = ResolveParameters ( methodInfo );

		return HasAssignableParametersType ( methodParameters , passingParametersTypes )
			&& HasSameParametersQuantity ( methodParameters , passingParametersTypes );

		static IEnumerable<Type> ResolveParameters ( MethodInfo methodInfo )
			=> methodInfo.GetParameters ()
				.Select ( parameterInfo => parameterInfo.ParameterType );

		static bool HasAssignableParametersType ( IEnumerable<Type> methodParametersTypes , IEnumerable<Type> passingParametersTypes )
		{
			var (methodParametersTypesEnumerator, passingParametersTypesEnumerator) =
				(methodParametersTypes.GetEnumerator (), passingParametersTypes.GetEnumerator ());

			while ( methodParametersTypesEnumerator.MoveNext () && passingParametersTypesEnumerator.MoveNext () )
			{
				var (methodParameterType, passingParameterType) =
					(methodParametersTypesEnumerator.Current, passingParametersTypesEnumerator.Current);

				if ( !methodParameterType.IsAssignableFrom ( passingParameterType ) )
					return false;
			}

			return true;
		}

		static bool HasSameParametersQuantity ( IEnumerable methodParametersTypes , IEnumerable passingParametersTypes )
			=> methodParametersTypes.Cast<object> ().Count ()
				== passingParametersTypes.Cast<object> ().Count ();
	}
}