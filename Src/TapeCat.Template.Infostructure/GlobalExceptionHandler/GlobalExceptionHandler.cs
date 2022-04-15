namespace TapeCat.Template.Infostructure.GlobalExceptionHandler;

using Domain.Shared.Common.Extensions;
using ExceptionHandlers;
using ExceptionHandlers.UnexpectableErrorHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

public sealed class GlobalExceptionHandler
{
	private const string JsonErrorMediaType = "application/problem+json";

	private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

	private readonly HttpContext _httpContext;

	internal GlobalExceptionHandler ( HttpContext? httpContext )
	{
		NotNull ( httpContext );

		_httpContext = InvokeJsonErrorMediaType ( ref httpContext! );
		_exceptionHandlers = ResolveErrorHandlers ( httpContext );

		static HttpContext InvokeJsonErrorMediaType ( ref HttpContext httpContext )
			=> httpContext.Tap ( httpContext =>
			  {
				  httpContext!.Response.ContentType = JsonErrorMediaType;
			  } )!;

		static IEnumerable<IExceptionHandler> ResolveErrorHandlers ( HttpContext httpContext )
			=> httpContext.RequestServices.GetRequiredService<IEnumerable<IExceptionHandler>> ();
	}

	public async Task FormErrorResponseAsync ( CancellationToken cancellationToken = default )
	{
		if ( TryHoldException ( out IExceptionHandler? exceptionHandler ) )
		{
			await FormExceptionHandlerErrorResponseAsync ( exceptionHandler! , cancellationToken );

			return;
		}

		await FormUnexpectableHandlerErrorResponseAsync ( cancellationToken );

		bool TryHoldException ( out IExceptionHandler? exceptionHandler )
		{
			var raisedException = ResolveRaisedException ();

			exceptionHandler =
				_exceptionHandlers
					.FirstOrDefault ( exceptionHandler => DoesErrorHandlerHoldException ( exceptionHandler , raisedException ) );

			return exceptionHandler is not null;

			static bool DoesErrorHandlerHoldException ( IExceptionHandler exceptionHandler , Exception raisedException ) =>
				exceptionHandler is not UnexpectableErrorHandler
					&& exceptionHandler.IsHold ( raisedException );

			Exception ResolveRaisedException ()
				=> _httpContext.ResolveException () ??
					throw new ArgumentException ( "No raised exception" );
		}

		async Task FormUnexpectableHandlerErrorResponseAsync ( CancellationToken cancellationToken = default )
		{
			var unexpectableErrorHandler = ResolveUnexpectableErrorHandler ();

			await FormExceptionHandlerErrorResponseAsync ( unexpectableErrorHandler , cancellationToken );

			IExceptionHandler ResolveUnexpectableErrorHandler ()
				=> _exceptionHandlers.SingleOrDefault ( exceptionHandler => exceptionHandler is UnexpectableErrorHandler ) ??
					throw new ArgumentException ( "No default error handler" );
		}

		async Task FormExceptionHandlerErrorResponseAsync ( IExceptionHandler exceptionHandler ,
																	CancellationToken cancellationToken = default )
		{
			_httpContext.Response.StatusCode = ( int ) exceptionHandler.StatusCode!;

			var errorResponse =
				exceptionHandler.FormExceptionMessage!.Invoke ( _httpContext );

			ExecuteExceptionHandlerCallback ( exceptionHandler );

			await _httpContext.Response.WriteAsync (
				text: JsonConvert.SerializeObject ( errorResponse ) ,
				cancellationToken );

			void ExecuteExceptionHandlerCallback ( IExceptionHandler exceptionHandler )
			{
				exceptionHandler.OnHold?.Invoke ( _httpContext );
			}
		}
	}
}