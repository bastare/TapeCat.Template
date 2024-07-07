namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers.Delegates;

using Microsoft.AspNetCore.Http;

public delegate Task OnExceptionHoldAsyncDelegate(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken = default);

public delegate T InjectExceptionMessageDelegate<out T>(
    Exception exception);

public delegate HttpStatusCode InjectStatusCodeDelegate(
    HttpContext httpContext,
    Exception exception);

public delegate bool IsAllowedExceptionDelegate(
    HttpContext httpContext,
    Exception exception);