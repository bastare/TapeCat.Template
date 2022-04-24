namespace TapeCat.Template.Infostructure.loC.Injectors;

using Domain.Shared.Common.Classes.HttpMessages.Error;
using Domain.Shared.Common.Exceptions;
using GlobalExceptionHandler;
using GlobalExceptionHandler.Builders;
using GlobalExceptionHandler.ExceptionHandlers;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class ErrorHandlerInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddSingleton ( implementationInstance: CreateGlobalExceptionHandlerBuilder () );

		static ExceptionHandlerManager CreateGlobalExceptionHandlerBuilder ()
			=> ExceptionHandlerManagerBuilder.Create ()
				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 1 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( FormatException ) )
					{
						InjectStatusCode = ( _ ) => HttpStatusCode.BadRequest ,
						InjectExceptionMessage =
							( httpContext ) =>
								new PageErrorMessage ()
								{
									Message = "Unexpected format" ,
									Description = "Sorry, try use other format." ,
									StatusCode = ( int ) HttpStatusCode.BadRequest ,
									TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
									ExceptionType = httpContext.ResolveExceptionTypeName () ,
									InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
									InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
								}
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 2 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( ForbiddenException ) )
					{
						InjectStatusCode = ( _ ) => HttpStatusCode.Forbidden ,
						InjectExceptionMessage =
							( httpContext ) =>
								new PageErrorMessage ()
								{
									Message = "Forbidden" ,
									Description = "User have no permission to this resource" ,
									StatusCode = ( int ) HttpStatusCode.Forbidden ,
									TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
									ExceptionType = httpContext.ResolveExceptionTypeName () ,
									InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
									InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
								}
					} )

					.WithErrorHandler (
						exceptionHandler: new ExceptionHandler (
							id: 3 ,
							isAllowedException: ( _ , exception ) =>
								exception.GetType () == typeof ( NotFoundException ) )
						{
							InjectStatusCode = ( _ ) => HttpStatusCode.NotFound ,
							InjectExceptionMessage = ( httpContext ) =>
								new PageErrorMessage ()
								{
									Message = "The requested url is not found" ,
									Description = "Sorry, the page you are looking for does not exist." ,
									StatusCode = ( int ) HttpStatusCode.NotFound ,
									TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
									ExceptionType = httpContext.ResolveExceptionTypeName () ,
									InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
									InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
								}
						} )

					.Build ();
	}
}