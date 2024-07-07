namespace TapeCat.Template.Domain.Shared.Common.Exceptions;

public sealed class NotFoundException : ArgumentNullException
{
    public NotFoundException(string? paramName = default, string? message = default)
        : base(paramName, message)
    { }

    public NotFoundException()
    { }

    public NotFoundException(string paramName)
        : base(paramName)
    { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    { }
}