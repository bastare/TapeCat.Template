namespace TapeCat.Template.Domain.Shared.Common.Interfaces;

public interface IBuilder<out TBuilded>
{
	TBuilded Build ();
}