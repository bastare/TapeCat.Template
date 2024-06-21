namespace TapeCat.Template.Api.Queries;

using Interfaces;

public sealed record ProjectionQuery : IProjectionQuery
{
	public string? Projection { get; init; }
}