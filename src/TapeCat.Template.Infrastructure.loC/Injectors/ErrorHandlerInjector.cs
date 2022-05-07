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
						InjectExceptionMessage = ( httpContext ) =>
							new PageErrorMessage (
								StatusCode: ( int ) HttpStatusCode.BadRequest ,
								Message: "Unexpected format" ,
								Description: "Sorry, try use other format." ,
								TechnicalErrorMessage: httpContext.ResolveExceptionMessage () ,
								ExceptionType: httpContext.ResolveExceptionTypeName () ,
								InnerMessage: httpContext.ResolveInnerExceptionMessage () ,
								InnerExceptionType: httpContext.ResolveInnerExceptionTypeName () )
					} )

				.WithErrorHandler (
					exceptionHandler: new ExceptionHandler (
						id: 2 ,
						isAllowedException: ( _ , exception ) =>
							exception.GetType () == typeof ( ForbiddenException ) )
					{
						InjectStatusCode = ( _ ) => HttpStatusCode.Forbidden ,
						InjectExceptionMessage = ( httpContext ) =>
							new PageErrorMessage (
								StatusCode: ( int ) HttpStatusCode.Forbidden ,
								Message: "Forbidden" ,
								Description: "User have no permission to this resource" ,
								TechnicalErrorMessage: httpContext.ResolveExceptionMessage () ,
								ExceptionType: httpContext.ResolveExceptionTypeName () ,
								InnerMessage: httpContext.ResolveInnerExceptionMessage () ,
								InnerExceptionType: httpContext.ResolveInnerExceptionTypeName () )
					} )

					.WithErrorHandler (
						exceptionHandler: new ExceptionHandler (
							id: 3 ,
							isAllowedException: ( _ , exception ) =>
								exception.GetType () == typeof ( NotFoundException ) )
						{
							InjectStatusCode = ( _ ) => HttpStatusCode.NotFound ,
							InjectExceptionMessage = ( httpContext ) =>
								new PageErrorMessage (
									StatusCode: ( int ) HttpStatusCode.NotFound ,
									Message: "The requested url is not found" ,
									Description: "Sorry, the page you are looking for does not exist." ,
									TechnicalErrorMessage: httpContext.ResolveExceptionMessage () ,
									ExceptionType: httpContext.ResolveExceptionTypeName () ,
									InnerMessage: httpContext.ResolveInnerExceptionMessage () ,
									InnerExceptionType: httpContext.ResolveInnerExceptionTypeName () )
						} )

					.WithErrorHandler (
						exceptionHandler: new ExceptionHandler (
							id: 4 ,
							isAllowedException: ( _ , exception ) =>
								exception.GetType () == typeof ( HttpRequestException ) )
						{
							InjectStatusCode = ( httpContext ) => httpContext.ResolveException<HttpRequestException> ()!.StatusCode!.Value ,
							InjectExceptionMessage = ( httpContext ) =>
								new PageErrorMessage (
									StatusCode: ( int ) httpContext.ResolveException<HttpRequestException> ()!.StatusCode!.Value ,
									Message: httpContext.ResolveExceptionMessage () )
						} )

					.Build ();
	}
}