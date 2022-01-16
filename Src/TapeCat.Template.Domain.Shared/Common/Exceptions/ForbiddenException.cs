namespace TapeCat.Template.Domain.Shared.Common.Exceptions;

using System;

public sealed class ForbiddenException : ArgumentException
{
	public ForbiddenException ()
	{ }

	public ForbiddenException ( string message )
		: base ( message )
	{ }

	public ForbiddenException ( string message , Exception innerException )
		: base ( message , innerException )
	{ }

	public ForbiddenException ( string message , string paramName )
		: base ( message , paramName )
	{ }

	public ForbiddenException ( string message , string paramName , Exception innerException )
		: base ( message , paramName , innerException )
	{ }
}