namespace TapeCat.Template.Domain.Shared.Helpers.AssertGuard.Common.Exceptions;

public sealed class AssertGuardException : Exception
{
	public AssertGuardException () { }

	public AssertGuardException ( string? message )
		: base ( message )
	{ }

	public AssertGuardException ( string? message , Exception? innerException )
		: base ( message , innerException )
	{ }
}