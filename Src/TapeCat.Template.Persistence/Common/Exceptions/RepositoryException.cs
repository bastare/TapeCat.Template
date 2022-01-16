namespace TapeCat.Template.Persistence.Common.Exceptions
{
	using System;

	public sealed class RepositoryException : Exception
	{
		public RepositoryException ()
		{ }

		public RepositoryException ( string? message = default )
			: base ( message )
		{ }

		public RepositoryException ( string message , Exception innerException )
			: base ( message , innerException )
		{ }
	}
}