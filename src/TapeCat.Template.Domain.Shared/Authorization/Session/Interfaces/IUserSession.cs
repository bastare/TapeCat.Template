namespace TapeCat.Template.Domain.Shared.Authorization.Session.Interfaces;

public interface IUserSession
{
	Guid? Id { get; }

	bool IsAuthorizedUser ();
}