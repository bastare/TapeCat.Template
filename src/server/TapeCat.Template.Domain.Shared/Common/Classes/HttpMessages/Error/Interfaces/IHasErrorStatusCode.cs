namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error.Interfaces;

public interface IHasErrorStatusCode
{
	public int? StatusCode { get; }
}