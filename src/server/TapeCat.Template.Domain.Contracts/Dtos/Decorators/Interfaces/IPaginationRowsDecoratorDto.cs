namespace TapeCat.Template.Domain.Contracts.Dtos.Decorators.Interfaces;

public interface IPaginationRowsDecoratorDto
{
	IEnumerable<object> Rows { get; }

	long CurrentOffset { get; }

	long TotalPages { get; }

	long Limit { get; }

	long TotalCount { get; }
}

public interface IPaginationRowsDecoratorDto<out T> : IPaginationRowsDecoratorDto
{
	new IEnumerable<T> Rows { get; }
}