namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard;

using Common.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Text;

public sealed class Guard : IGuard
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public Guard ( IHttpContextAccessor httpContextAccessor )
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public static void NotNullOrEmpty<TException> ( string? @string ,
													string variableName = IGuard.DefaultVariableName ,
													string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null or empty";

		if ( string.IsNullOrEmpty ( @string ) )
			RaiseException<TException> ( variableName , message );
	}

	public void Validate<TValidator, TValue, TException> ( TValue value , string variableName = IGuard.DefaultVariableName )
		where TValidator : IValidator<TValue>
		where TException : ArgumentException
	{
		var validator = ResolveValidator<TValidator , TValue> ();

		var validationResult = validator.Validate ( value );

		if ( !validationResult.IsValid )
		{
			var errorMessage = ResolveErrorMessage ( validationResult );

			RaiseException<TException> ( variableName , errorMessage );
		}
	}

	private static string ResolveErrorMessage ( ValidationResult validationResult ) =>
		new StringBuilder ()
			.AppendJoin (
				separator: "\n" ,
				values: validationResult.Errors )

			.ToString ();

	private TValidator ResolveValidator<TValidator, TValue> ()
		where TValidator : IValidator<TValue>
			=> _httpContextAccessor.HttpContext!.RequestServices
				.GetRequiredService<TValidator> ();

	public static void NotNullOrEmpty ( string? @string , string variableName = IGuard.DefaultVariableName , string? message = default )
	{
		message ??= $"`{variableName}` is null or empty";

		if ( string.IsNullOrEmpty ( @string ) )
			throw new ArgumentNullException ( variableName , message );
	}

	public static void NotNull<TException> ( object? value , string variableName = IGuard.DefaultVariableName , string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null";

		if ( value is null )
			RaiseException<TException> ( variableName , message );
	}

	public static void NotNull ( object? value , string variableName = IGuard.DefaultVariableName , string? message = default )
	{
		message ??= $"`{variableName}` is null";

		if ( value is null )
			throw new ArgumentNullException ( variableName , message );
	}

	public static void NotNullOrEmpty<TException> ( IEnumerable? collection ,
													string variableName = IGuard.DefaultVariableName ,
													string? message = default )
		where TException : ArgumentNullException
	{
		message ??= $"`{variableName}` is null or empty";

		if ( EnumerableExtensions.IsNullOrEmpty ( collection ) )
			RaiseException<TException> ( variableName , message );
	}

	public static void NotNullOrEmpty ( IEnumerable? collection , string variableName = IGuard.DefaultVariableName , string? message = default )
	{
		message ??= $"`{variableName}` is null or empty";

		if ( collection.IsNullOrEmpty () )
			throw new ArgumentNullException ( variableName , message );
	}

	private static void RaiseException<TException> ( string variableName , string message )
		where TException : ArgumentException
	{
		var exception =
			CreateException<TException> ( variableName , message );

		throw exception;
	}

	private static TException CreateException<TException> ( string? variableName , string? message )
		where TException : ArgumentException
			=> ( TException ) Activator.CreateInstance ( typeof ( TException ) , variableName , message )!;
}