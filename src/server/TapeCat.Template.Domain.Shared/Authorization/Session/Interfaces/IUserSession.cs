namespace TapeCat.Template.Domain.Shared.Authorization.Session.Interfaces;

public interface IUserSession<TKey> : IUserSession
{
	new TKey Id { get; }
}

public interface IUserSession
{
	object Id { get; }

	bool IsAuthorizedUser ();
}