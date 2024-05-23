namespace TapeCat.Template.Domain.Shared.Authorization.Session;

using Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

public sealed class UserSession ( IHttpContextAccessor httpContextAccessor ) : IUserSession
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

	public Guid? Id =>
		HttpContext.User
			.FindFirst ( ClaimTypes.NameIdentifier )?.Value is string guid
				? Guid.Parse ( guid )
				: default;

	public bool IsAuthorizedUser ()
		=> Guid.TryParse (
			input: HttpContext.User
				.FindFirst ( ClaimTypes.NameIdentifier )?.Value ,

			result: out Guid _ );
}