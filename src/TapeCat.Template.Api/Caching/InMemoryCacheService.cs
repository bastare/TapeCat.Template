namespace TapeCat.Template.Api.Caching;

using Interfaces;
using Microsoft.Extensions.Caching.Memory;

public sealed class InMemoryCacheService : ICacheService
{
	private readonly IMemoryCache _memoryCache;

	public InMemoryCacheService ( IMemoryCache memoryCache )
	{
		_memoryCache = memoryCache;
	}

	public Task<TCachedValue> GetAsync<TCachedValue> ( string key , CancellationToken cancellationToken = default )
		=> Task.FromResult (
			result: _memoryCache.Get<TCachedValue> ( key ) );

	public Task SetAsync<TCachedValue> ( string key , TCachedValue value , TimeSpan expireSpan , CancellationToken cancellationToken = default )
	{
		_memoryCache.Set ( key , value , expireSpan );

		return Task.CompletedTask;
	}

	public bool TryGet<TCachedValue> ( string key , out TCachedValue value )
		=> _memoryCache.TryGetValue ( key , out value );

	public async Task<TCachedValue> GetOrCreateCacheValueAsync<TCachedValue> (
		string key ,
		TCachedValue value ,
		TimeSpan expireSpan ,
		CancellationToken cancellationToken = default )
	{
		NotNullOrEmpty ( key );

		return await _memoryCache.GetOrCreateAsync (
			key ,
			factory: ( cacheEntry ) =>
			  {
				  cacheEntry
					.SetSlidingExpiration ( expireSpan )
					.SetValue ( value );

				  return Task.FromResult ( value );
			  } );
	}

	public void Remove ( string key )
	{
		_memoryCache.Remove ( key );
	}
}