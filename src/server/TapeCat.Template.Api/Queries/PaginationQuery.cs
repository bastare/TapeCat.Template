namespace TapeCat.Template.Api.Queries;

using Interfaces;
using System.ComponentModel.DataAnnotations;

public sealed record PaginationQuery : IPaginationQuery
{
	[Range ( 1 , int.MaxValue )]
	public int? Offset { get; init; } = 1;

	[Range ( 1 , int.MaxValue )]
	public int? Limit { get; init; } = 10;
}