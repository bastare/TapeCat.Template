namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers.NotFoundHandlers;

using Domain.Shared.Common.Classes.HttpMessages;
using Domain.Shared.Common.Exceptions;
using Domain.Shared.Common.Extensions;
using System.Net;
using TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers;

public sealed record NotFoundHandler : ExceptionHandler<NotFoundException>
{
	public NotFoundHandler ()
		: base ( HttpStatusCode.NotFound )
	{
		FormExceptionMessage =
			httpContext =>
				new ErrorMessage ()
				{
					Message = "The requested url is not found" ,
					Description = "Sorry, the page you are looking for does not exist." ,
					StatusCode = ( int ) HttpStatusCode.NotFound ,
					IsErrorPage = true ,
					TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
					ExceptionType = httpContext.ResolveExceptionTypeName () ,
					InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
					InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
				};
	}
}