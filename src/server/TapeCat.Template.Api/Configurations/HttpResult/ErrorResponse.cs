namespace TapeCat.Template.Api.Configurations.HttpResult;

using Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public readonly struct ErrorResponse ( Exception? exception ) : IResult
{
	private readonly Exception _exception = NotNull ( exception );

	public async Task ExecuteAsync ( HttpContext httpContext )
	{
		await GlobalExceptionHandlerConfigurator.ExceptionFiltersConfigurator (
			httpContext ,
			_exception );
	}
}