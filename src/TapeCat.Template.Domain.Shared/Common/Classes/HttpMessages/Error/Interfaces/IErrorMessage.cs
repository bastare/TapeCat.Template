namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error.Interfaces;

public interface IErrorMessage
{
	int StatusCode { get; }

	bool IsErrorPage { get; }

	string? Message { get; }

	string? Description { get; }
}