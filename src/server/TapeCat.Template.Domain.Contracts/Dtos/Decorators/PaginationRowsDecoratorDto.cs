namespace TapeCat.Template.Domain.Contracts.Dtos.Decorators;

using Interfaces;

public sealed record PaginationRowsDecoratorDto<T> : IPaginationRowsDecoratorDto<T>
	where T : class
{
	public IEnumerable<T> Rows { get; init; } = [];

	public long CurrentOffset { get; init; }

	public long TotalPages { get; init; }

	public long Limit { get; init; }

	public long TotalCount { get; init; }

	IEnumerable<object> IPaginationRowsDecoratorDto.Rows => Rows;
}