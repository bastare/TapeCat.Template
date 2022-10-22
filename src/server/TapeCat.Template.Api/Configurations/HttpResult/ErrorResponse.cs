namespace TapeCat.Template.Api.Configurations.HttpResult;

using Infrastructure.CrossCutting.Configurators.ExceptionHandlerConfigurators;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

public readonly struct ErrorResponse : IResult
{
	private readonly Exception _exception;

	public ErrorResponse ( Exception exception )
		=> _exception = exception;

	public Task ExecuteAsync ( HttpContext httpContext )
	{
		throw new NotImplementedException ();
	}
}