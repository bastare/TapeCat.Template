namespace TapeCat.Template.Api.Common.Extensions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public static class WebHostEnvironmentExtensions
{
	public static bool IsStage ( this IWebHostEnvironment webHostEnvironment )
		=> webHostEnvironment.IsEnvironment ( "Stage" );

	public static bool IsProduction ( this IWebHostEnvironment webHostEnvironment )
		=> webHostEnvironment.IsEnvironment ( "Production" );

	public static bool IsDevelopment ( this IWebHostEnvironment webHostEnvironment )
		=> webHostEnvironment.IsEnvironment ( "Development" );
}