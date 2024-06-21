namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Domain.Shared.Common.Classes.HttpMessages.Error;
using Domain.Shared.Common.Exceptions;
using GlobalExceptionHandler;
using GlobalExceptionHandler.Builders;
using GlobalExceptionHandler.ExceptionHandlers;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

public sealed class ErrorHandlerInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddSingleton ( implementationInstance: CreateGlobalExceptionHandlerManager () );

		static ExceptionHandlerManager CreateGlobalExceptionHandlerManager ()
			=> ExceptionHandlerManagerBuilder.Create ()
				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 1 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( FormatException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.BadRequest ,
						InjectExceptionMessage = ( _ ) =>
							new PageErrorMessage (
								StatusCode: ( int ) HttpStatusCode.BadRequest ,
								Message: "Unexpected format" ,
								Description: "Sorry, try use other format." )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 2 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( ForbiddenException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.Forbidden ,
						InjectExceptionMessage = ( _ ) =>
							new PageErrorMessage (
								StatusCode: ( int ) HttpStatusCode.Forbidden ,
								Message: "Forbidden" ,
								Description: "User have no permission to this resource" )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 3 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( NotFoundException ) )
					{
						InjectStatusCode = ( _ , _ ) => HttpStatusCode.NotFound ,
						InjectExceptionMessage = ( _ ) =>
							new PageErrorMessage (
								StatusCode: ( int ) HttpStatusCode.NotFound ,
								Message: "The requested url is not found" ,
								Description: "Sorry, the page you are looking for does not exist." )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 4 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( HttpRequestException ) )
					{
						InjectStatusCode = ( httpContext , _ ) => httpContext.ResolveException<HttpRequestException> ()!.StatusCode!.Value ,
						InjectExceptionMessage = ( exception ) =>
							new PageErrorMessage (
								StatusCode: ( int ) ( ( HttpRequestException ) exception ).StatusCode!.Value ,
								Message: exception.Message )
					} )

				.Build ();
	}
}