namespace TapeCat.Template.Domain.Shared.Authorization.Session;

using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

public sealed class UserSession : ValueObject<UserSession>
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public Guid? Id =>
		_httpContextAccessor.HttpContext!.User
			.FindFirst ( ClaimTypes.NameIdentifier )?.Value is string guid
				? Guid.Parse ( guid )
				: default;

	public UserSession ( IHttpContextAccessor httpContextAccessor )
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public bool IsAuthorizedUser ()
		=> Guid.TryParse (
			input: _httpContextAccessor.HttpContext!.User
				.FindFirst ( ClaimTypes.NameIdentifier )?.Value ,

			result: out Guid _ );
}
