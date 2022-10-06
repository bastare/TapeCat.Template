namespace TapeCat.Template.Infrastructure.CrossCutting.Configurators.SwaggerConfigurators;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;

public static class SwaggerConfigurator
{
	public static void SwaggerDIConfigurator ( SwaggerGenOptions swaggerGenOptions )
	{
		swaggerGenOptions
			.AddSecurityRequirements ()
			.AddSecurityDefinitions ()
			.AddSwaggerDocs ()

			.OperationFilter<ApiVersionOperationFilter> ();
	}

	private static SwaggerGenOptions AddSecurityRequirements ( this SwaggerGenOptions swaggerGenOptions )
	{
		swaggerGenOptions.AddSecurityRequirement (
			securityRequirement: new ()
			{
				{
					new ()
					{
						Reference =
							new ()
							{
								Type = ReferenceType.SecurityScheme ,
								Id = "Bearer"
							} ,
						Scheme = "oauth2" ,
						Name = "Bearer" ,
						In = ParameterLocation.Header
					} ,
					new List<string> ()
				}
			} );

		return swaggerGenOptions;
	}

	private static SwaggerGenOptions AddSecurityDefinitions ( this SwaggerGenOptions swaggerGenOptions )
	{
		swaggerGenOptions.AddSecurityDefinition (
			name: "Bearer" ,
			securityScheme: new ()
			{
				Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"" ,
				Name = "Authorization" ,
				In = ParameterLocation.Header ,
				Type = SecuritySchemeType.ApiKey ,
				Scheme = "Bearer"
			} );

		return swaggerGenOptions;
	}

	private static SwaggerGenOptions AddSwaggerDocs ( this SwaggerGenOptions swaggerGenOptions )
	{
		swaggerGenOptions.SwaggerDoc (
			name: "v1" ,
			info: new ()
			{
				Version = "v1"
			} );

		return swaggerGenOptions;
	}

	public static void SwaggerUIConfigurator ( SwaggerUIOptions swaggerUIOptions )
	{
		swaggerUIOptions.SwaggerEndpoint ( "/swagger/v1/swagger.json" , string.Empty );

		swaggerUIOptions.RoutePrefix = "swagger";
	}
}