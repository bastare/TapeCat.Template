namespace TapeCat.Template.Infostructure.GlobalExceptionHandler;

using Domain.Shared.Common.Extensions;
using ExceptionHandlers;
using ExceptionHandlers.UnexpectableErrorHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Domain.Shared.Helpers.AssertGuard.Guard;

public sealed class GlobalExceptionHandler
{
	private const string JsonErrorMediaType = "application/problem+json";

	private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

	private readonly HttpContext _httpContext;

	internal GlobalExceptionHandler ( HttpContext? httpContext )
	{
		NotNull ( httpContext , nameof ( httpContext ) );

		_httpContext = InvokeJsonErrorMediaType ( ref httpContext! );
		_exceptionHandlers = ResolveErrorHandlers ( httpContext );

		static HttpContext InvokeJsonErrorMediaType ( ref HttpContext httpContext ) =>
			httpContext.Tap ( httpContext =>
			  {
				  httpContext.Response.ContentType = JsonErrorMediaType;
			  } );

		static IEnumerable<IExceptionHandler> ResolveErrorHandlers ( HttpContext httpContext )
		{
			using var scopeService = httpContext.RequestServices.CreateScope ();

			return scopeService.ServiceProvider.GetRequiredService<IEnumerable<IExceptionHandler>> ();
		}
	}

	public async Task FormErrorResponseAsync ( CancellationToken cancellationToken = default )
	{
		if ( TryHoldException ( out IExceptionHandler? exceptionHandler ) )
			await FormExceptionHandlerErrorResponseAsync ( exceptionHandler! , cancellationToken );
		else
			await FormUnexpectableHandlerErrorResponseAsync ( cancellationToken );
	}

	private bool TryHoldException ( out IExceptionHandler? exceptionHandler )
	{
		var raisedException = ResolveRaisedException ();

		exceptionHandler =
			_exceptionHandlers
				.FirstOrDefault ( exceptionHandler => DoesErrorHandlerHoldException ( exceptionHandler , raisedException ) );

		return exceptionHandler is not null;
	}

	private static bool DoesErrorHandlerHoldException ( IExceptionHandler exceptionHandler , Exception raisedException ) =>
		exceptionHandler is not UnexpectableErrorHandler
			&& exceptionHandler.IsHold ( raisedException );

	private Exception ResolveRaisedException ()
		=> _httpContext.ResolveException () ??
			throw new ArgumentNullException ( "No raised exception" );

	private async Task FormUnexpectableHandlerErrorResponseAsync ( CancellationToken cancellationToken = default )
	{
		var unexpectableErrorHandler = ResolveUnexpectableErrorHandler ();

		await FormExceptionHandlerErrorResponseAsync ( unexpectableErrorHandler , cancellationToken );
	}

	private IExceptionHandler ResolveUnexpectableErrorHandler ()
		=> _exceptionHandlers.SingleOrDefault ( exceptionHandler => exceptionHandler is UnexpectableErrorHandler ) ??
			throw new ArgumentNullException ( "No default error handler" );

	private async Task FormExceptionHandlerErrorResponseAsync ( IExceptionHandler exceptionHandler ,
																CancellationToken cancellationToken = default )
	{
		_httpContext.Response.StatusCode = ( int ) exceptionHandler.StatusCode!;

		var errorResponse =
			exceptionHandler.FormExceptionMessage!.Invoke ( _httpContext );

		ExecuteExceptionHandlerCallback ( exceptionHandler );

		await _httpContext.Response.WriteAsync (
			text: JsonConvert.SerializeObject ( errorResponse ) ,
			cancellationToken );
	}

	private void ExecuteExceptionHandlerCallback ( IExceptionHandler exceptionHandler )
	{
		exceptionHandler.OnHold?.Invoke ( _httpContext );
	}
}