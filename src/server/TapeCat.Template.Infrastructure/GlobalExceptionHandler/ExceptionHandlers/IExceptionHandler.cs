namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers;

using Microsoft.AspNetCore.Http;
using System.Net;

public interface IExceptionHandler<out TErrorMessage> : IExceptionHandler
{
	new Func<Exception , TErrorMessage> InjectExceptionMessage { get; }
}

public interface IExceptionHandler
{
	int Id { get; }

	Action<HttpContext , Exception>? OnHold { get; }

	Func<Exception , object> InjectExceptionMessage { get; }

	Func<HttpContext , Exception , HttpStatusCode> InjectStatusCode { get; }

	bool IsHold ( HttpContext context , Exception exception );
}