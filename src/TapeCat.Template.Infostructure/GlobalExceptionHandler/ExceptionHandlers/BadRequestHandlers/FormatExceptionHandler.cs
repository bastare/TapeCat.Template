namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers.BadRequestHandlers;

using Domain.Shared.Common.Classes.HttpMessages;
using Domain.Shared.Common.Extensions;
using System;
using System.Net;

public sealed class FormatExceptionHandler : ExceptionHandler
{
	public FormatExceptionHandler ()
		: base (
			isAllowedException: ( _ , exception ) =>
				exception.GetType () == typeof ( FormatException ) )
	{
		StatusCode = HttpStatusCode.BadRequest;

		FormExceptionMessage =
			( httpContext ) =>
				new ErrorMessage ()
				{
					Message = "Unexpected format" ,
					Description = "Sorry, try use other format." ,
					StatusCode = ( int ) HttpStatusCode.BadRequest ,
					IsErrorPage = true ,
					TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
					ExceptionType = httpContext.ResolveExceptionTypeName () ,
					InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
					InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
				};
	}
}