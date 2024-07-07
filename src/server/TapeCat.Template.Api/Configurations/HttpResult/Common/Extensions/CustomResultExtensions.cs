namespace TapeCat.Template.Api.Configurations.HttpResult.Common.Extensions;

using Microsoft.AspNetCore.Http;

public static class CustomResultExtensions
{
    public static ErrorResponse ErrorResponse(this IResultExtensions _, Exception exception)
        => new(exception);
}