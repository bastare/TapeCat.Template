namespace TapeCat.Template.Contracts.Dtos.WrapDtos;

using Interfaces;

public sealed record PaginationRowsDto<T> : IPaginationRowsDto<T>
	where T : class
{
	public IEnumerable<T> Rows { get; init; } = Enumerable.Empty<T> ();

	public int CurrentOffset { get; init; }

	public int TotalPages { get; init; }

	public int Limit { get; init; }

	public int TotalCount { get; init; }

	IEnumerable<object> IPaginationRowsDto.Rows => Rows;
}