namespace TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers.BadRequestHandlers
{
	using System;
	using System.Net;
	using TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages;
	using TapeCat.Template.Domain.Shared.Common.Extensions;
	using TapeCat.Template.Infostructure.GlobalExceptionHandler.ExceptionHandlers;

	public sealed record FormatExceptionHandler : ExceptionHandler<FormatException>
	{
		public FormatExceptionHandler ()
			: base ( HttpStatusCode.BadRequest )
		{
			FormExceptionMessage =
				httpContext =>
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
}