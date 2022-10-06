namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error;

using Interfaces;

public sealed record PageErrorMessage (
	int? StatusCode = 500 ,
	string? Message = default ,
	string? Description = default ,
	string? TechnicalErrorMessage = default ,
	string? ExceptionType = default ,
	string? InnerMessage = default ,
	string? InnerExceptionType = default ) :
		IHasErrorDescription,
		IHasErrorStatusCode,
		IHasErrorPage,
		IHasExceptionInformation
{
	public bool IsErrorPage => true;
}