namespace TapeCat.Template.Domain.Contracts.Dtos.Decorators;

using Interfaces;

public sealed record EffectedRowsDecoratorDto<T> : IEffectedRowsDecoratorDto<T>
	where T : class
{
	public IEnumerable<T> Rows { get; init; } = [];

	public long EffectedRows { get; init; }

	IEnumerable<object> IEffectedRowsDecoratorDto.Rows => Rows;
}