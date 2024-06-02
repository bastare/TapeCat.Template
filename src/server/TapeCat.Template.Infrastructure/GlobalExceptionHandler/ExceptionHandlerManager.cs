namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler;

using Domain.Shared.Common.Classes.HttpMessages.Error;
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
			if ( !AreUnique ( exceptionHandlers ) )
				throw new ArgumentException ( "There are 1 or more error handler(-s), that have duplicated `id`" );

			static bool AreUnique ( IEnumerable<IExceptionHandler> exceptionHandlers )
				=> exceptionHandlers.DistinctBy ( exceptionHandler => exceptionHandler.Id ).Count ()
					== exceptionHandlers.Count ();
		}
	}

	public async Task FormErrorResponseAsync ( HttpContext? httpContext , Exception? exception )
	{
		try
		{
			NotNull ( httpContext );
			NotNull ( exception );

			InjectJsonErrorMediaType ( httpContext! );

			if ( TryHoldException ( out IExceptionHandler? exceptionHandler , httpContext! , exception! ) )
			{
				await FormExceptionHandlerErrorResponseAsync (
					exceptionHandler! ,
					httpContext! ,
					exception! ,
					cancellationToken: httpContext!.RequestAborted );

				return;
			}

			await FormUnexpectableHandlerErrorResponseAsync (
				httpContext! ,
				exception! ,
				cancellationToken: httpContext!.RequestAborted );
		}
		catch ( Exception )
		{
			await FormInnerErrorResponseAsync (
				httpContext! ,
				cancellationToken: httpContext!.RequestAborted );
		}

		static void InjectJsonErrorMediaType ( HttpContext httpContext )
		{
			httpContext.Response.ContentType = JsonErrorMediaType;
		}

		bool TryHoldException ( out IExceptionHandler? exceptionHandler , HttpContext httpContext , Exception exception )
		{
			exceptionHandler = ResolveExceptionHandlersThatHoldRaisedException ( httpContext , exception );

			return exceptionHandler is not null;

			IExceptionHandler? ResolveExceptionHandlersThatHoldRaisedException ( HttpContext httpContext , Exception exception )
			{
				return ResolveSingleExceptionHandler (
					exceptionHandlers: ResolveExceptionHandlersThatHoldRaisedException ( httpContext , exception ) ,
					exception );

				IEnumerable<IExceptionHandler> ResolveExceptionHandlersThatHoldRaisedException ( HttpContext httpContext , Exception exception )
					=> _exceptionHandlers
						.Where ( exceptionHandler =>
							exceptionHandler.IsHold ( httpContext , exception ) );

				static IExceptionHandler? ResolveSingleExceptionHandler ( IEnumerable<IExceptionHandler> exceptionHandlers , Exception exception )
					=> exceptionHandlers.Count () switch
					{
						1 => exceptionHandlers.First (),
						0 => default,
						> 1 => throw new ArgumentException ( message: CreateErrorMessage ( exceptionHandlers , exception ) , nameof ( exceptionHandlers ) ),
						_ => throw new ArgumentException ( message: $"No case for this condition: {exceptionHandlers.Count ()}" , nameof ( exceptionHandlers ) )
					};

				static string CreateErrorMessage ( IEnumerable<IExceptionHandler> exceptionHandlers , Exception exception )
					=> new StringBuilder ()
						.Append ( "There are collision between 2 or more exception handlers, on " )
						.Append ( exception.GetType ().ShortDisplayName () )
						.Append ( ", between: " )
						.AppendJoin (
							separator: ", " ,
							exceptionHandlers.Select ( exceptionHandler => exceptionHandler.Id ) )
						.Append ( " - error handler" )

						.ToString ();
			}
		}

		async Task FormUnexpectableHandlerErrorResponseAsync ( HttpContext httpContext , Exception exception , CancellationToken cancellationToken = default )
		{
			await FormExceptionHandlerErrorResponseAsync (
				exceptionHandler: _defaultExceptionHandler ,
				httpContext ,
				exception ,
				cancellationToken );
		}

		static async Task FormExceptionHandlerErrorResponseAsync ( IExceptionHandler exceptionHandler ,
																   HttpContext httpContext ,
																   Exception exception ,
																   CancellationToken cancellationToken = default )
		{
			InvokeStatusCode ( exceptionHandler , httpContext , exception );

			ExecuteExceptionHandlerCallback ( exceptionHandler , httpContext , exception );

			await FormErrorMessageAsync ( exceptionHandler , httpContext , exception , cancellationToken );

			static void InvokeStatusCode ( IExceptionHandler exceptionHandler , HttpContext httpContext , Exception exception )
			{
				httpContext!.Response.StatusCode = ( int ) exceptionHandler.InjectStatusCode.Invoke ( httpContext , exception );
			}

			static void ExecuteExceptionHandlerCallback ( IExceptionHandler exceptionHandler , HttpContext httpContext , Exception exception )
			{
				exceptionHandler.OnHold?.Invoke ( httpContext , exception );
			}

			static async Task FormErrorMessageAsync ( IExceptionHandler exceptionHandler ,
													  HttpContext httpContext ,
													  Exception exception ,
													  CancellationToken cancellationToken = default )
			{
				await httpContext.Response.WriteAsync (
					text: JsonConvert.SerializeObject ( value: ResolveExceptionMessage ( exceptionHandler , exception ) ) ,
					cancellationToken );

				static object ResolveExceptionMessage ( IExceptionHandler exceptionHandler , Exception exception )
					=> exceptionHandler.InjectExceptionMessage.Invoke ( exception );
			}
		}

		static async Task FormInnerErrorResponseAsync ( HttpContext httpContext ,
														CancellationToken cancellationToken = default )
		{
			httpContext!.Response.StatusCode = ( int ) HttpStatusCode.InternalServerError;

			await httpContext.Response.WriteAsync (
				text: JsonConvert.SerializeObject (
					value: new ErrorMessage (
						Message: "Internal server error" ,
						StatusCode: ( int ) HttpStatusCode.InternalServerError ) ) ,
				cancellationToken );
		}
	}
}