namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard.Common.Exceptions;

public sealed class AssertValidationException : ArgumentException
{
	public IEnumerable<string> ErrorMessages { get; } = Enumerable.Empty<string> ();

	public AssertValidationException ()
	{ }

	public AssertValidationException ( IEnumerable<string> errorMessages )
	{
		ErrorMessages = errorMessages;
	}

	public AssertValidationException ( string? message )
		: base ( message )
	{ }

	public AssertValidationException ( string? message , Exception? innerException )
		: base ( message , innerException )
	{ }

	public AssertValidationException ( string? message , string? paramName )
		: base ( message , paramName )
	{ }

	public AssertValidationException ( string? message , string? paramName , Exception? innerException )
		: base ( message , paramName , innerException )
	{ }
}