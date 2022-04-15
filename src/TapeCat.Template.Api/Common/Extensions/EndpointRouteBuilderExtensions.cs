namespace TapeCat.Template.Api.Common.Extensions;

using Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Reflection;

public static class EndpointRouteBuilderExtensions
{
	public static void MapHub<THub> ( this IEndpointRouteBuilder endpointRouteBuilder )
		where THub : Hub
	{
		var routePath = ResolveHubRoutePath ( typeof ( THub ) );

		endpointRouteBuilder.MapHub<THub> ( routePath );
	}

	private static string ResolveHubRoutePath ( Type hubType )
		=> hubType.GetCustomAttribute<SignalRHubRouteAttribute> ()?.Path ??
			throw new ArgumentNullException ( nameof ( hubType ) , $"No required attribute: {nameof ( SignalRHubRouteAttribute )}" );
}