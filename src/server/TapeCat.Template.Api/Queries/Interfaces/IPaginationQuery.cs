namespace TapeCat.Template.Api.Queries.Interfaces;

public interface IPaginationQuery
{
	int? Offset { get; }

	int? Limit { get; }
}