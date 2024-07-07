namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record PageErrorMessage(
    int? StatusCode = 500,
    string? Message = default,
    string? Description = default) :
        IHasErrorDescription,
        IHasErrorStatusCode,
        IHasErrorPage
{
    public bool IsErrorPage => true;
}