namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers;

using Delegates;
using Domain.Shared.Common.Classes.HttpMessages.Error;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

public sealed record ExceptionHandler : IExceptionHandler
{
    public int Id { get; }

    public OnExceptionHoldAsyncDelegate? OnHoldAsync { get; init; }

    public InjectExceptionMessageDelegate<object> InjectExceptionMessage { get; init; } = DefaultExceptionMessageInjector;

    public InjectStatusCodeDelegate InjectStatusCode { get; init; } = DefaultStatusCodeInjector;

    private IsAllowedExceptionDelegate IsAllowedException { get; }

    public ExceptionHandler(int id, IsAllowedExceptionDelegate? isAllowedException)
    {
        NotNull(isAllowedException);

        Id = id;
        IsAllowedException = isAllowedException!;
    }

    public bool IsHold(HttpContext httpContext, Exception exception)
        => IsAllowedException.Invoke(httpContext, exception);

    private static object DefaultExceptionMessageInjector(Exception exception)
        => new PageErrorMessage(
            Message: "Internal server error",
            Description: "Sorry, something went wrong on our end. We are currently trying to fix the problem");

    private static HttpStatusCode DefaultStatusCodeInjector(HttpContext _, Exception _1)
        => HttpStatusCode.InternalServerError;
}