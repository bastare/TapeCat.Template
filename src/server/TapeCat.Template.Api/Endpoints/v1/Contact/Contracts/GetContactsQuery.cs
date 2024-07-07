namespace TapeCat.Template.Api.Endpoints.v1.Contact.Contracts;

using Queries.Interfaces;

public sealed record GetContactsQuery :
	IExpressionQuery,
	IOrderQuery,
	IPaginationQuery,
	IProjectionQuery
{
	public string? Expression { get; init; }

	public string? Projection { get; init; }

	public int? Offset { get; init; } = 1;

	public int? Limit { get; init; } = 10;

	public bool? IsDescending { get; init; }

	public string? OrderBy { get; init; }
}