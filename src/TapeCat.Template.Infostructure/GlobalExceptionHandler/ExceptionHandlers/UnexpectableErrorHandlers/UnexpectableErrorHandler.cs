namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers.UnexpectableErrorHandlers;

using Domain.Shared.Common.Classes.HttpMessages;
using Domain.Shared.Common.Extensions;

public sealed class UnexpectableErrorHandler : ExceptionHandler
{
	public UnexpectableErrorHandler ()
		: base (
			isAllowedException: ( httpContext , _ ) =>
				httpContext.ResolveException () is not null )
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