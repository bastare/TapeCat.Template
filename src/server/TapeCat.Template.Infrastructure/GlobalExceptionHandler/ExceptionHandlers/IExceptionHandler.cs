namespace TapeCat.Template.Infrastructure.GlobalExceptionHandler.ExceptionHandlers;

using Delegates;
using Microsoft.AspNetCore.Http;

public interface IExceptionHandler<out TErrorMessage> : IExceptionHandler
{
	new InjectExceptionMessageDelegate<TErrorMessage> InjectExceptionMessage { get; }
}

public interface IExceptionHandler
{
	int Id { get; }

	OnExceptionHoldAsyncDelegate? OnHoldAsync { get; }

	InjectExceptionMessageDelegate<object> InjectExceptionMessage { get; }

	InjectStatusCodeDelegate InjectStatusCode { get; }

	bool IsHold ( HttpContext context , Exception exception );
}