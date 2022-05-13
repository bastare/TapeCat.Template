namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler;

using Domain.Shared.Common.Classes.HttpMessages.Error;
using Domain.Shared.Common.Extensions;
using ExceptionHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;

public sealed class ExceptionHandlerManager
{
	private const string JsonErrorMediaType = "application/problem+json";

	private readonly ImmutableList<IExceptionHandler> _exceptionHandlers;

	private readonly IExceptionHandler _defaultExceptionHandler;

	internal ExceptionHandlerManager (
		ImmutableList<IExceptionHandler> exceptionHandlers ,
		IExceptionHandler defaultExceptionHandler )
	{
		ExceptionHandlersAreUniq ( exceptionHandlers );

		_exceptionHandlers = exceptionHandlers;
		_defaultExceptionHandler = defaultExceptionHandler;

		static void ExceptionHandlersAreUniq ( IEnumerable<IExceptionHandler> exceptionHandlers )
		{
			if ( !AreUniqua ( exceptionHandlers ) )
				throw new ArgumentException ( "There are 1 or more error handler(-s), that have duplicated `id`" );

			static bool AreUniqua ( IEnumerable<IExceptionHandler> exceptionHandlers )
				=> exceptionHandlers.DistinctBy ( exceptionHandler => exceptionHandler.Id ).Count ()
					== exceptionHandlers.Count ();
		}
	}

	public async Task FormErrorResponseAsync ( HttpContext? httpContext )
	{
		try
		{
			NotNull ( httpContext );

			InjectJsonErrorMediaType ( ref httpContext! );

			if ( TryHoldException ( out IExceptionHandler? exceptionHandler , httpContext ) )
			{
				await FormExceptionHandlerErrorResponseAsync (
					exceptionHandler! ,
					httpContext ,
					cancellationToken: httpContext.RequestAborted );

				return;
			}

			await FormUnexpectableHandlerErrorResponseAsync ( cancellationToken: httpContext.RequestAborted );
		}
		catch ( Exception exception )
		{
			await FormInnerErrorResponseAsync ( exception , httpContext! , cancellationToken: httpContext!.RequestAborted );
		}

		static void InjectJsonErrorMediaType ( ref HttpContext httpContext )
		{
			httpContext.Response.ContentType = JsonErrorMediaType;
		}

		bool TryHoldException ( out IExceptionHandler? exceptionHandler , HttpContext httpContext )
		{
			exceptionHandler = ResolveExceptionHandlersThatHoldRaisedException ( httpContext );

			return exceptionHandler is not null;

			IExceptionHandler? ResolveExceptionHandlersThatHoldRaisedException ( HttpContext httpContext )
			{
				return ResolveSingleExceptionHandler (
					exceptionHandlers: ResolveExceptionHandlersThatHoldRaisedException ( httpContext ) ,
					httpContext );

				IEnumerable<IExceptionHandler> ResolveExceptionHandlersThatHoldRaisedException ( HttpContext httpContext )
					=> _exceptionHandlers
						.Where ( exceptionHandler =>
							exceptionHandler.IsHold ( httpContext , exception: ResolveRaisedException ( httpContext ) ) );

				static IExceptionHandler? ResolveSingleExceptionHandler ( IEnumerable<IExceptionHandler> exceptionHandlers , HttpContext httpContext )
					=> exceptionHandlers.Count () switch
					{
						1 => exceptionHandlers.First (),
						0 => default,
						> 1 => throw new ArgumentException ( message: CreateErrorMessage ( exceptionHandlers , httpContext ) , nameof ( exceptionHandlers ) ),
						_ => throw new ArgumentException ( message: $"No case for this condition: {exceptionHandlers.Count ()}" , nameof ( exceptionHandlers ) )
					};

				static string CreateErrorMessage ( IEnumerable<IExceptionHandler> exceptionHandlers , HttpContext httpContext )
					=> new StringBuilder ()
						.Append ( "There are colision between 2 or more exception handlers, on " )
						.Append ( ResolveRaisedException ( httpContext ).GetType ().ShortDisplayName () )
						.Append ( ", between: " )
						.AppendJoin (
							separator: ", " ,
							exceptionHandlers.Select ( exceptionHandler => exceptionHandler.Id ) )
						.Append ( " - error handler" )

						.ToString ();

				static Exception ResolveRaisedException ( HttpContext httpContext )
					=> httpContext.ResolveException () ??
						throw new ArgumentException ( "No raised exception" , nameof ( httpContext ) );
			}
		}

		async Task FormUnexpectableHandlerErrorResponseAsync ( CancellationToken cancellationToken = default )
		{
			await FormExceptionHandlerErrorResponseAsync (
				exceptionHandler: _defaultExceptionHandler ,
				httpContext ,
				cancellationToken );
		}

		static async Task FormExceptionHandlerErrorResponseAsync ( IExceptionHandler exceptionHandler ,
																   HttpContext httpContext ,
																   CancellationToken cancellationToken = default )
		{
			InvokeStatusCode ( exceptionHandler , httpContext );

			ExecuteExceptionHandlerCallback ( exceptionHandler , httpContext );

			await FormErrorMessageAsync ( exceptionHandler , httpContext , cancellationToken );

			static void InvokeStatusCode ( IExceptionHandler exceptionHandler , HttpContext httpContext )
			{
				httpContext!.Response.StatusCode = ( int ) exceptionHandler.InjectStatusCode.Invoke ( httpContext );
			}

			static void ExecuteExceptionHandlerCallback ( IExceptionHandler exceptionHandler , HttpContext httpContext )
			{
				exceptionHandler.OnHold?.Invoke ( httpContext );
			}

			static async Task FormErrorMessageAsync ( IExceptionHandler exceptionHandler ,
													  HttpContext httpContext ,
													  CancellationToken cancellationToken = default )
			{
				await httpContext.Response.WriteAsync (
					text: JsonConvert.SerializeObject ( value: ResolveExceptionMessage ( exceptionHandler , httpContext ) ) ,
					cancellationToken );
			}

			static object ResolveExceptionMessage ( IExceptionHandler exceptionHandler , HttpContext httpContext )
				=> exceptionHandler.InjectExceptionMessage.Invoke ( httpContext );
		}

		static async Task FormInnerErrorResponseAsync ( Exception innerException ,
														HttpContext httpContext ,
														CancellationToken cancellationToken = default )
		{
			httpContext!.Response.StatusCode = ( int ) HttpStatusCode.InternalServerError;

			await httpContext.Response.WriteAsync (
				text: JsonConvert.SerializeObject (
					value: new PageErrorMessage (
						StatusCode: ( int ) HttpStatusCode.InternalServerError ,
						TechnicalErrorMessage: innerException.Message ,
						ExceptionType: innerException.GetType ().ShortDisplayName () ) ) ,
				cancellationToken );
		}
	}
}