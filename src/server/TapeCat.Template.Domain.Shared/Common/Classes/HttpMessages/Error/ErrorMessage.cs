namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record ErrorMessage(
    string? Message = default,
    int? StatusCode = default,
    string? Description = default) :
        IHasErrorStatusCode, IHasErrorDescription, IHasErrorPage
{
    public bool IsErrorPage => false;
}