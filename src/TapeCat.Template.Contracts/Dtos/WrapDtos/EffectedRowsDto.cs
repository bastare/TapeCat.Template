namespace TapeCat.Template.Contracts.Dtos.WrapDtos;

using Interfaces;

public sealed record EffectedRowsDto<T> : IEffectedRowsDto<T>
	where T : class
{
	public IEnumerable<T> Rows { get; init; } = Enumerable.Empty<T> ();

	public long EffectedRows { get; init; }

	IEnumerable<object> IEffectedRowsDto.Rows => Rows;
}