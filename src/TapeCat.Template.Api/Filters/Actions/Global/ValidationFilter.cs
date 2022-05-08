namespace TapeCat.Template.Api.Filters.Actions.Global;

using Domain.Shared.Common.Classes.HttpMessages.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Threading.Tasks;

public sealed class ValidationFilter : IAsyncActionFilter
{
	private const string JsonErrorMediaType = "application/problem+json";

	public async Task OnActionExecutionAsync ( ActionExecutingContext actionExecutingContext , ActionExecutionDelegate next )
	{
		if ( actionExecutingContext.ModelState.IsValid )
		{
			await next ();

			return;
		}

		FormErrorResponse ( ref actionExecutingContext );

		static void FormErrorResponse ( ref ActionExecutingContext actionExecutingContext )
		{
			actionExecutingContext.Result = new ContentResult
			{
				ContentType = JsonErrorMediaType ,
				StatusCode = StatusCodes.Status400BadRequest ,
				Content = CreateErrorContent ( actionExecutingContext )
			};

			static string CreateErrorContent ( ActionExecutingContext actionExecutingContext )
				=> JsonConvert.SerializeObject (
					value: new PageErrorMessageWrap (
						ErrorMessages: CreateInnerErrorMessages ( actionExecutingContext.ModelState ) ) );

			static ImmutableList<InnerErrorMessage> CreateInnerErrorMessages ( ModelStateDictionary modelStateDictionary )
				=> modelStateDictionary
					.Where ( HasErrors )
					.Select ( ResolveNestedErrorMessage )
					.Select ( ResolveErrorMessage )
					.ToImmutableList ();

			static bool HasErrors ( KeyValuePair<string , ModelStateEntry?> keyValuePair )
				=> keyValuePair.Value?.Errors.Count > 0;

			static IEnumerable<string> ResolveNestedErrorMessage ( KeyValuePair<string , ModelStateEntry?> keyValuePair )
				=> keyValuePair.Value!.Errors.Select ( error => error.ErrorMessage );

			static InnerErrorMessage ResolveErrorMessage ( IEnumerable<string> errorMessages )
				=> new ( Message: string.Concat ( errorMessages ) );
		}
	}
}