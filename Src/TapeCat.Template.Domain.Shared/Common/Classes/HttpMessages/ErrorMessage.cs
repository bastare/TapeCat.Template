namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages;

public sealed record ErrorMessage
{
	public int StatusCode { get; init; }

	public bool IsErrorPage { get; init; }

	public string? Message { get; init; }

	public string? Description { get; init; }

	public string? TechnicalErrorMessage { get; init; }

	public string? ExceptionType { get; init; }

	public string? InnerMessage { get; init; }

	public string? InnerExceptionType { get; init; }
}