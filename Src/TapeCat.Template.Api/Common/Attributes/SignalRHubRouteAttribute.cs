namespace TapeCat.Template.Api.Common.Attributes;

using System;
using static Domain.Shared.Helpers.AssertGuard.Guard;

[AttributeUsage ( AttributeTargets.Class , AllowMultiple = false )]
public sealed class SignalRHubRouteAttribute : Attribute
{
	public string Path { get; }

	public SignalRHubRouteAttribute ( string? path )
	{
		NotNullOrEmpty ( path , nameof ( path ) );

		Path = path!;
	}
}