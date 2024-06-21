namespace TapeCat.Template.Domain.Shared.Authorization.Session;

using Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

public sealed class UserSession<TKey> ( IHttpContextAccessor httpContextAccessor ) : IUserSession<TKey>
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

	public TKey Id => default!;

	object IUserSession.Id => Id!;

	public bool IsAuthorizedUser ()
		=> Guid.TryParse (
			input: HttpContext.User
				.FindFirst ( ClaimTypes.NameIdentifier )?.Value ,

			result: out Guid _ );
}