namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard;

using FluentValidation;
using FluentValidation.Results;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TapeCat.Template.Domain.Shared.Common.Extensions;

public sealed class Guard : IGuard
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public Guard ( IHttpContextAccessor httpContextAccessor )
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public static string NotNullOrEmpty<TException> ( string? @string ,
													  [CallerArgumentExpression ( "string" )] string? variableName = default ,
													  string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null or empty";

		if ( string.IsNullOrEmpty ( @string ) )
			RaiseException<TException> ( variableName! , message );

		return @string!;
	}

	public TValue Validate<TValidator, TValue, TException> ( TValue value , [CallerArgumentExpression ( "value" )] string? variableName = default )
		where TValidator : IValidator<TValue>
		where TException : ArgumentException
	{
		var validationResult = ResolveValidator ()
			.Validate ( value );

		if ( validationResult is { IsValid: false } )
			RaiseValidatorException ( variableName! , validationResult );

		return value;

		TValidator ResolveValidator ()
			=> _httpContextAccessor.HttpContext!.RequestServices
				.GetRequiredService<TValidator> ();

		static void RaiseValidatorException ( string variableName , ValidationResult validationResult )
		{
			RaiseException<TException> (
				variableName ,
				message: ResolveErrorMessage ( validationResult ) );

			static string ResolveErrorMessage ( ValidationResult validationResult )
				=> new StringBuilder ()
					.AppendJoin (
						separator: "\n" ,
						values: validationResult.Errors )

					.ToString ();
		}
	}

	public static string NotNullOrEmpty ( string? @string ,
										  [CallerArgumentExpression ( "string" )] string? variableName = default ,
										  string? message = default )
	{
		message ??= $"`{variableName}` is null or empty";

		if ( string.IsNullOrEmpty ( @string ) )
			throw new ArgumentNullException ( variableName , message );

		return @string!;
	}

	public static object NotNull<TException> ( object? value ,
											   [CallerArgumentExpression ( "value" )] string? variableName = default ,
											   string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null";

		if ( value is null )
			RaiseException<TException> ( variableName! , message );

		return value!;
	}

	public static object NotNull ( object? value ,
								   [CallerArgumentExpression ( "value" )] string? variableName = default ,
								   string? message = default )
	{
		message ??= $"`{variableName}` is null";

		if ( value is null )
			throw new ArgumentNullException ( variableName , message );

		return value!;
	}

	public static IEnumerable NotNullOrEmpty<TException> ( IEnumerable? collection ,
														   [CallerArgumentExpression ( "collection" )] string? variableName = default ,
														   string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null or empty";

		if ( collection.IsNullOrEmpty () )
			RaiseException<TException> ( variableName! , message );

		return collection!;
	}

	public static IEnumerable NotNullOrEmpty ( IEnumerable? collection ,
											   [CallerArgumentExpression ( "collection" )] string? variableName = default ,
											   string? message = default )
	{
		message ??= $"`{variableName}` is null or empty";

		if ( collection.IsNullOrEmpty () )
			throw new ArgumentNullException ( variableName , message );

		return collection!;
	}

	private static void RaiseException<TException> ( string variableName , string message )
		where TException : ArgumentException
	{
		throw CreateException ( variableName , message );

		static TException CreateException ( string? variableName , string? message )
			=> ( TException ) Activator.CreateInstance ( typeof ( TException ) , variableName , message )!;
	}
}