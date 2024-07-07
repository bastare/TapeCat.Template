namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;

public static class HttpContextExtensions
{
    public static Exception? ResolveException(this HttpContext httpContext) =>
        httpContext.Features.Get<IExceptionHandlerPathFeature>()
            ?.Error;

    public static TException? ResolveException<TException>(this HttpContext httpContext)
        where TException : Exception
            => httpContext.Features.Get<IExceptionHandlerPathFeature>()
                ?.Error as TException;

    public static string? ResolveExceptionMessage(this HttpContext httpContext) =>
        httpContext.ResolveException()
            ?.Message;

    public static string? ResolveExceptionTypeName(this HttpContext httpContext) =>
        httpContext.ResolveException()
            ?.GetType()
                .ShortDisplayName();

    public static string? ResolveInnerExceptionMessage(this HttpContext httpContext) =>
        httpContext.ResolveException()
            ?.InnerException?.Message;

    public static string? ResolveInnerExceptionTypeName(this HttpContext httpContext) =>
        httpContext.ResolveException()
            ?.InnerException?.GetType()
                .ShortDisplayName();
}