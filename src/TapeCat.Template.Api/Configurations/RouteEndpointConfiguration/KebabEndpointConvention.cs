namespace TapeCat.Template.Api.Configurations.RouteEndpointConfiguration;

using CaseExtensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using System;

public sealed class KebabEndpointConvention : RouteTokenTransformerConvention
{
	public KebabEndpointConvention ()
		: base ( parameterTransformer: new KebabEndpointTransformer () )
	{ }

	private sealed class KebabEndpointTransformer : IOutboundParameterTransformer
	{
		public string TransformOutbound ( object? routeEndpoint )
			=> ToKebabCase ( routeEndpoint );

		private static string ToKebabCase ( object? routeEndpoint )
			=> ( routeEndpoint as string )?.ToKebabCase () ??
				throw new ArgumentNullException ( nameof ( routeEndpoint ) , "Route endpoint is null" );
	}
}
