namespace TapeCat.Template.Domain.Contracts.Dtos.WrapDtos.Interfaces;

public interface IPaginationRowsDto
{
	IEnumerable<object> Rows { get; }

	int CurrentOffset { get; }

	int TotalPages { get; }

	int Limit { get; }

	int TotalCount { get; }
}

public interface IPaginationRowsDto<out T> : IPaginationRowsDto
{
	new IEnumerable<T> Rows { get; }
}