namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers;

using Microsoft.AspNetCore.Http;
using System.Net;

public interface IExceptionHandler<out IErrorMessage> : IExceptionHandler
{
	new Func<Exception , IErrorMessage> InjectExceptionMessage { get; }
}

public interface IExceptionHandler
{
	int Id { get; }

	Action<HttpContext , Exception>? OnHold { get; }

	Func<Exception , object> InjectExceptionMessage { get; }

	Func<HttpContext , Exception , HttpStatusCode> InjectStatusCode { get; }

	bool IsHold ( HttpContext context , Exception exception );
}