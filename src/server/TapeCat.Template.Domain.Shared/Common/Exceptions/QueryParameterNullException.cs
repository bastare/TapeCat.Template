namespace TapeCat.Template.Domain.Shared.Common.Exceptions;

public sealed class QueryParameterNullException : ArgumentNullException
{
    public QueryParameterNullException(string? message = default, string? parameterName = default)
        : base(message, parameterName)
    { }

    public QueryParameterNullException()
    { }

    public QueryParameterNullException(string paramName)
        : base(paramName)
    { }

    public QueryParameterNullException(string message, Exception innerException)
        : base(message, innerException)
    { }
}