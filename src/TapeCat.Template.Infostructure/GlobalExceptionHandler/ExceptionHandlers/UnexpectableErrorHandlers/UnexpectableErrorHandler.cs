namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers.UnexpectableErrorHandlers;

using Domain.Shared.Common.Classes.HttpMessages;
using Domain.Shared.Common.Extensions;
using System;
using System.Net;

public sealed record UnexpectableErrorHandler : ExceptionHandler<Exception>
{
	public UnexpectableErrorHandler ()
		: base ( HttpStatusCode.InternalServerError )
	{
		FormExceptionMessage =
			httpContext =>
				new ErrorMessage
				{
					Message = "Internal server error" ,
					Description = "Sorry, something went wrong on our end. We are currently trying to fix the problem" ,
					StatusCode = httpContext.Response.StatusCode ,
					IsErrorPage = true ,
					TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
					ExceptionType = httpContext.ResolveExceptionTypeName () ,
					InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
					InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
				};
	}
}