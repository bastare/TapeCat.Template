namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers;

using Microsoft.AspNetCore.Http;
using System;
using System.Net;

public interface IExceptionHandler
{
	Func<HttpContext , object>? FormExceptionMessage { get; }

	HttpStatusCode StatusCode { get; }

	Action<HttpContext>? OnHold { get; }

	bool IsHold ( Exception exception );
}