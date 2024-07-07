namespace TapeCat.Template.Domain.Shared.Authorization.Session;

using Interfaces;
using Microsoft.AspNetCore.Http;
public sealed class UserSession<TKey> ( IHttpContextAccessor httpContextAccessor ) : IUserSession<TKey>
{
	private readonly IHttpContextAccessor _ = httpContextAccessor;

	public TKey Id => default!;

	object IUserSession.Id => Id!;

	public bool IsAuthorizedUser ()
		=> false;
}