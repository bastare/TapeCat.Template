namespace TapeCat.Template.Infostructure.loC.Injectors;

using GlobalExceptionHandler.ExceptionHandlers;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class ErrorHandlersInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.Scan ( scan =>
		  {
			  scan.FromAssemblyOf<IExceptionHandler> ()
				  .AddClasses ( classes => { classes.AssignableTo<IExceptionHandler> (); } )

					  .AsImplementedInterfaces ()

					  .WithSingletonLifetime ();
		  } );
	}
}