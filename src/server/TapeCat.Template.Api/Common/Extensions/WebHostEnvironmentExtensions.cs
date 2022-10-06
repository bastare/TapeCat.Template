namespace TapeCat.Template.Api.Common.Extensions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public static class WebHostEnvironmentExtensions
{
	public static bool IsStage ( this IWebHostEnvironment webHostEnvironment )
		=> webHostEnvironment.IsEnvironment ( "Stage" );
}