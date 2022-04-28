namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers;

using Domain.Shared.Common.Classes.HttpMessages.Error;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

public sealed record ExceptionHandler : IExceptionHandler
{
	public int Id { get; }

	public Action<HttpContext>? OnHold { get; init; }

	public Func<HttpContext , object> InjectExceptionMessage { get; init; } = DefaultExceptionMessageInjector;

	public Func<HttpContext , HttpStatusCode> InjectStatusCode { get; init; } = DefaultStatusCodeInjector;

	private Func<HttpContext , Exception , bool> IsAllowedException { get; }

	public ExceptionHandler ( int id , Func<HttpContext , Exception , bool>? isAllowedException )
	{
		NotNull ( isAllowedException );

		IsAllowedException = isAllowedException!;
		Id = id;
	}

	public bool IsHold ( HttpContext context , Exception exception )
		=> IsAllowedException.Invoke ( context , exception );

	private static object DefaultExceptionMessageInjector ( HttpContext httpContext )
		=> new PageErrorMessage
		{
			Message = "Internal server error" ,
			Description = "Sorry, something went wrong on our end. We are currently trying to fix the problem" ,
			StatusCode = httpContext.Response.StatusCode ,
			TechnicalErrorMessage = httpContext.ResolveExceptionMessage () ,
			ExceptionType = httpContext.ResolveExceptionTypeName () ,
			InnerMessage = httpContext.ResolveInnerExceptionMessage () ,
			InnerExceptionType = httpContext.ResolveInnerExceptionTypeName ()
		};

	private static HttpStatusCode DefaultStatusCodeInjector ( HttpContext _ )
		=> HttpStatusCode.InternalServerError;
}