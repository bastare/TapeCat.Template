namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record PageErrorMessage : IErrorMessage
{
	public int StatusCode { get; init; }

	public bool IsErrorPage => true;

	public string? Message { get; init; }

	public string? Description { get; init; }

	public string? TechnicalErrorMessage { get; init; }

	public string? ExceptionType { get; init; }

	public string? InnerMessage { get; init; }

	public string? InnerExceptionType { get; init; }
}