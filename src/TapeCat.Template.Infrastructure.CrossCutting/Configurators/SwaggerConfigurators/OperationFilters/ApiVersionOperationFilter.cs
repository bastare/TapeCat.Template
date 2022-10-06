namespace TapeCat.Template.Infrastructure.CrossCutting.Configurators.SwaggerConfigurators.OperationFilters;

using Asp.Versioning;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ApiVersionOperationFilter : IOperationFilter
{
	public void Apply ( OpenApiOperation operation , OperationFilterContext context )
	{
		var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
		operation.Parameters ??= new List<OpenApiParameter> ();

		var hasApiVersionMetadata =
			actionMetadata.Any ( metadataItem => metadataItem is ApiVersionMetadata );

		if ( hasApiVersionMetadata )
		{
			operation.Parameters.Add ( new OpenApiParameter
			{
				Name = "v" ,
				In = ParameterLocation.Query ,
				Description = "API Version value" ,
				Schema = new OpenApiSchema
				{
					Type = "String" ,
					Default = new OpenApiString ( "1.0" )
				}
			} );
		}
	}
}