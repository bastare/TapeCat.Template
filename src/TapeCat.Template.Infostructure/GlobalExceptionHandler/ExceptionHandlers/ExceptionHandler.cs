namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers;

using Microsoft.AspNetCore.Http;
using System;
using System.Net;

public abstract class ExceptionHandler : IExceptionHandler
{
	public Action<HttpContext>? OnHold { get; }

	public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.InternalServerError;

	public Func<HttpContext , object> FormExceptionMessage { get; protected set; } = DefaultExceptionMessage;

	protected Func<HttpContext , Exception , bool> IsAllowedException { get; }

	protected ExceptionHandler ( Func<HttpContext , Exception , bool> isAllowedException )
	{
		IsAllowedException = isAllowedException;
	}

	public bool IsHold ( HttpContext context , Exception exception )
		=> IsAllowedException.Invoke ( context , exception );

	public virtual void InjectStatusCode ( HttpContext _ )
	{
		StatusCode = HttpStatusCode.InternalServerError;
	}

	private static object DefaultExceptionMessage ( HttpContext _ )
		=> new
		{
			Message = "Internal server error" ,
			Description = "Sorry, something went wrong on our end. We are currently trying to fix the problem" ,
			StatusCode = HttpStatusCode.InternalServerError ,
			IsErrorPage = true
		};
}