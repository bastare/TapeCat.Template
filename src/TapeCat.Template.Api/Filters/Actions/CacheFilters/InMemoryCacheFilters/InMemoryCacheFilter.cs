namespace TapeCat.Template.Api.Filters.Actions.CacheFilters.InMemoryCacheFilters;

using Caching.Intrefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Net.Mime;

[AttributeUsage ( AttributeTargets.Method , AllowMultiple = false )]
public sealed class InMemoryCacheFilter : Attribute, IAsyncActionFilter
{
	private const string TimeSpanFormat = @"d\.hh\:mm";

	private const double MaxCacheExpirationDuration = 365;

	private readonly TimeSpan _cacheLiveTime;

	public bool IsUserRelated { get; set; }

	public InMemoryCacheFilter ()
	{
		_cacheLiveTime = TimeSpan.FromDays ( MaxCacheExpirationDuration );
	}

	public InMemoryCacheFilter ( string cacheLiveTime )
	{
		_cacheLiveTime = ConvertToTimeSpanDuration ( cacheLiveTime );

		static TimeSpan ConvertToTimeSpanDuration ( string cacheLiveTime )
		{
			NotNullOrEmpty ( cacheLiveTime );

			if ( TimeSpan.TryParseExact ( cacheLiveTime , TimeSpanFormat , CultureInfo.CurrentCulture , out TimeSpan timeSpan ) )
				return timeSpan;

			throw new ArgumentException ( $"Unvalid form of timespam: {cacheLiveTime} (format `{TimeSpanFormat}`)" , nameof ( cacheLiveTime ) );
		}
	}

	public async Task OnActionExecutionAsync ( ActionExecutingContext actionExecutingContext , ActionExecutionDelegate next )
	{
		if ( !IsGetRequestMethod ( actionExecutingContext ) )
			throw new ArgumentException ( $"Cache filter can be applied only for `{HttpMethod.Get}` actions" );

		var cacheService = ResolveMemoryCacheService ( actionExecutingContext );

		var cacheKey = GenerateCacheKey ( actionExecutingContext );

		if ( cacheService.TryGet ( cacheKey , out object value ) )
		{
			actionExecutingContext.Result = new ContentResult
			{
				Content = JsonConvert.SerializeObject ( value ) ,
				ContentType = MediaTypeNames.Application.Json ,
				StatusCode = StatusCodes.Status200OK
			};

			return;
		}

		await SetCacheValueAsync ( next , cacheService , cacheKey );

		static bool IsGetRequestMethod ( ActionExecutingContext actionExecutingContext )
			=> actionExecutingContext.HttpContext.Request.Method == HttpMethod.Get.ToString ();

		static ICacheService ResolveMemoryCacheService ( ActionExecutingContext actionExecutingContext )
			=> actionExecutingContext.HttpContext.RequestServices.GetRequiredService<ICacheService> ();

		string GenerateCacheKey ( ActionExecutingContext actionExecutingContext )
		{
			var cacheKeyGenerator = CacheKeyGenerator.Create ( actionExecutingContext );

			return IsUserRelated
				? cacheKeyGenerator.GenerateUserRelatedCacheKey ()
				: cacheKeyGenerator.GenerateUserNonRelatedCacheKey ();
		}

		async Task SetCacheValueAsync (
			ActionExecutionDelegate next ,
			ICacheService cacheService ,
			string cacheKey ,
			CancellationToken cancellationToken = default )
		{
			var executedContext = await next ();

			if ( executedContext.Result is OkObjectResult okObjectResult )
				await cacheService.SetAsync ( cacheKey , okObjectResult.Value , _cacheLiveTime , cancellationToken );
		}
	}
}