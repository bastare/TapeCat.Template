namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record ErrorMessage (
	string? Message ,
	int StatusCode ,
	string? Description );

public sealed record ErrorMessageWrap : IErrorMessage
{
	public int StatusCode { get; init; }

	public bool IsErrorPage { get; init; }

	public string? Message { get; init; }

	public string? Description { get; init; }

	public List<ErrorMessage> ErrorMessages { get; } = new ();
}