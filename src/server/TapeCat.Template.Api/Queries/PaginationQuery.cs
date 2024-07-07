namespace TapeCat.Template.Api.Queries;

using Interfaces;

public sealed record PaginationQuery : IPaginationQuery
{
	public int? Offset { get; init; } = 1;

	public int? Limit { get; init; } = 10;
}