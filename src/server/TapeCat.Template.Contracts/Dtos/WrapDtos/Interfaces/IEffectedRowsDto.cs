namespace TapeCat.Template.Contracts.Dtos.WrapDtos.Interfaces;

public interface IEffectedRowsDto
{
	IEnumerable<object> Rows { get; }

	long EffectedRows { get; }
}

public interface IEffectedRowsDto<out T> : IEffectedRowsDto
{
	new IEnumerable<T> Rows { get; }
}