namespace TapeCat.Template.Infrastructure.Persistence.Specifications.DynamicLinqDecorator;

public sealed class LinqSyntaxException : ArgumentException
{
	public LinqSyntaxException ()
	{ }

	public LinqSyntaxException ( string? message )
		: base ( message )
	{ }

	public LinqSyntaxException ( string? message , Exception? innerException )
		: base ( message , innerException )
	{ }

	public LinqSyntaxException ( string? message , string? paramName )
		: base ( message , paramName )
	{ }

	public LinqSyntaxException ( string? message , string? paramName , Exception? innerException )
		: base ( message , paramName , innerException )
	{ }
}