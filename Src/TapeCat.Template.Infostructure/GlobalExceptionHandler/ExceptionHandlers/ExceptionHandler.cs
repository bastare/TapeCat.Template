namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers;

using Microsoft.AspNetCore.Http;
using System;
using System.Net;

public abstract record ExceptionHandler<TException> : IExceptionHandler
	where TException : Exception
{
	public HttpStatusCode StatusCode { get; }

	public Func<HttpContext , object>? FormExceptionMessage { get; protected init; }

	protected Predicate<TException>? DoesHoldException { get; init; }

	public Action<HttpContext>? OnHold { get; protected init; }

	public ExceptionHandler ()
		=> StatusCode = HttpStatusCode.InternalServerError;

	public ExceptionHandler ( HttpStatusCode statusCode )
		=> StatusCode = statusCode;

	public bool IsHold ( Exception exception )
		=> exception is TException expectedException
			&& IsAllowedException ( expectedException );

	private bool IsAllowedException ( TException raisedException )
		=> DoesHoldException?.Invoke ( raisedException ) ?? true;
}