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

		actionExecutingContext.Result = new ContentResult
		{
			ContentType = JsonErrorMediaType ,
			StatusCode = StatusCodes.Status400BadRequest ,
			Content = JsonConvert.SerializeObject (
				new PageErrorMessageWrap (
					ErrorMessages: CreateInnerErrorMessages (
						errorMessages: ResolveErrorMessages ( actionExecutingContext.ModelState )! ) ) )
		};

		static IEnumerable<IEnumerable<string>?>? ResolveErrorMessages ( ModelStateDictionary modelStateDictionary )
			=> modelStateDictionary
				.Where ( keyValuePair => keyValuePair.Value?.Errors.Count > 0 )
				.Select ( keyValuePair =>
					keyValuePair.Value?.Errors
						.Select ( error => error.ErrorMessage ) );

		static ImmutableList<InnerErrorMessage> CreateInnerErrorMessages ( IEnumerable<IEnumerable<string>> errorMessages )
			=> errorMessages
				.Select ( errorMessage =>
					new InnerErrorMessage (
						Message: string.Concat ( errorMessage! ) ) )

				.ToImmutableList ();
	}
}