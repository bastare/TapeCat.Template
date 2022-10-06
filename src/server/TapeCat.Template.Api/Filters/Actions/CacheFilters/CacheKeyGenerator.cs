namespace TapeCat.Template.Api.Filters.Actions.CacheFilters;

using Domain.Shared.Authorization.Session;
using Domain.Shared.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

public sealed class CacheKeyGenerator
{
	private readonly ActionExecutingContext _actionExecutingContext;

	private readonly UserSession _userSession;

	private string RequestPath => _actionExecutingContext.HttpContext.Request.Path;

	private string RequestQuery => _actionExecutingContext.HttpContext.Request.QueryString.ToString ();

	private CacheKeyGenerator ( ActionExecutingContext actionExecutingContext )
	{
		_actionExecutingContext = actionExecutingContext;
		_userSession = ResolveUserSession ();

		UserSession ResolveUserSession ()
			=> _actionExecutingContext.HttpContext.RequestServices
				.GetRequiredService<UserSession> ();
	}

	public static CacheKeyGenerator Create ( ActionExecutingContext actionExecutingContext )
		=> new ( actionExecutingContext );

	public string GenerateUserRelatedCacheKey ()
	{
		return string.Concat ( RequestPath , ResolveUserId () , RequestQuery )
			.ToSHA256 ();

		string ResolveUserId ()
			=> _userSession.Id?.ToString () ??
				throw new ArgumentException ( "No authorization token" );
	}

	public string GenerateUserNonRelatedCacheKey ()
		=> string.Concat ( RequestPath , RequestQuery )
			.ToSHA256 ();
}