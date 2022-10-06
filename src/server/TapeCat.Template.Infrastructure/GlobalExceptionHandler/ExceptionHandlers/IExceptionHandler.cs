namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers;

using Microsoft.AspNetCore.Http;
using System.Net;

public interface IExceptionHandler<out IErrorMessage> : IExceptionHandler
{
	new Func<HttpContext , IErrorMessage> InjectExceptionMessage { get; }
}

public interface IExceptionHandler
{
	int Id { get; }

	Func<HttpContext , object> InjectExceptionMessage { get; }

	Func<HttpContext , HttpStatusCode> InjectStatusCode { get; }

	Action<HttpContext>? OnHold { get; }

	bool IsHold ( HttpContext context , Exception exception );
}