namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.Factories;

using Microsoft.AspNetCore.Http;

public static class GlobalExceptionHandlerFactory
{
	public static GlobalExceptionHandler Create ( HttpContext httContext )
		=> new ( httContext );
}