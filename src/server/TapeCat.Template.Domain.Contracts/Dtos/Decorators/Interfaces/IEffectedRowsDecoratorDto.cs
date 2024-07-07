namespace TapeCat.Template.Domain.Contracts.Dtos.Decorators.Interfaces;

public interface IEffectedRowsDecoratorDto
{
	IEnumerable<object> Rows { get; }

	long EffectedRows { get; }
}

public interface IEffectedRowsDecoratorDto<out T> : IEffectedRowsDecoratorDto
{
	new IEnumerable<T> Rows { get; }
}