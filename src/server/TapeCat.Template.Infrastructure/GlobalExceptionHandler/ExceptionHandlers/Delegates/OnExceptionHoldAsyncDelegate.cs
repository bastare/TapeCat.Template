namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers.Delegates;

using Microsoft.AspNetCore.Http;

public delegate Task OnExceptionHoldAsync (
	HttpContext httpContext ,
	Exception exception ,
	CancellationToken cancellationToken = default );