namespace TapeCat.Template.Domain.Shared.Common.Classes.HttpMessages.Error.Interfaces;

public interface IHasErrorDescription
{
	public string? Message { get; }

	public string? Description { get; }
}