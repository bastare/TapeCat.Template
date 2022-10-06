namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error.Interfaces;

public interface IHasExceptionInformation
{
	string? TechnicalErrorMessage { get; }

	string? ExceptionType { get; }

	string? InnerMessage { get; }

	string? InnerExceptionType { get; }
}