namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers.ForbiddenHandlers;

using Domain.Shared.Common.Classes.HttpMessages;
using Domain.Shared.Common.Exceptions;
using Domain.Shared.Common.Extensions;
using System.Net;

public sealed record InnerForbiddenHandler : ExceptionHandler<MassTransit.RequestException>
{
	public InnerForbiddenHandler ()
		: base ( HttpStatusCode.NotFound )
	{
		DoesHoldException =
			requestException =>
				requestException?.InnerException is ForbiddenException;

		FormExceptionMessage =
			httpContext =>
				new ErrorMessage ()
				{
					Message = "Forbidden" ,
					Description = "User have no permission to this resource" ,
					StatusCode = ( int ) HttpStatusCode.Forbidden ,
					IsErrorPage = true ,
					TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
					ExceptionType = httpContext.ResolveExceptionTypeName () ,
					InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
					InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
				};
	}
}
