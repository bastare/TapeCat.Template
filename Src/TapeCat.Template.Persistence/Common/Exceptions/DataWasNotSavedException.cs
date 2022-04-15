namespace TapeCat.Template.Persistence.Common.Exceptions;

public sealed class DataWasNotSavedException : Exception
{
	private const string ExceptionMessage = "Data wasn`t saved";

	public DataWasNotSavedException ()
		: base ( ExceptionMessage )
	{ }

	public DataWasNotSavedException ( string message )
		: base ( message )
	{ }

	public DataWasNotSavedException ( string message , Exception innerException )
		: base ( message , innerException )
	{ }
}