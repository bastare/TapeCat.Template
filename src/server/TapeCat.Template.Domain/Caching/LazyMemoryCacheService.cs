namespace TapeCat.Template.Domain.Caching;

using Interfaces;
using LazyCache;

public sealed class LazyMemoryCacheService ( IAppCache memoryCache ) : ICacheService
{
	private readonly IAppCache _memoryCache = memoryCache;

	public async Task<TCachedValue?> GetAsync<TCachedValue> ( string key , CancellationToken _ = default )
		=> await _memoryCache.GetAsync<TCachedValue> ( key );

	public Task SetAsync<TCachedValue> ( string key , TCachedValue value , TimeSpan expireSpan , CancellationToken _ = default )
	{
		_memoryCache.Add ( key , value , expireSpan );

		return Task.CompletedTask;
	}

	public bool TryGet<TCachedValue> ( string key , out TCachedValue value )
		=> _memoryCache.TryGetValue ( key , out value );

	public Task<TCachedValue> GetOrCreateCacheValueAsync<TCachedValue> (
		string key ,
		Func<Task<TCachedValue>> factoryAsync ,
		TimeSpan expireSpan ,
		CancellationToken _ = default )
	{
		NotNullOrEmpty ( key );

		return _memoryCache.GetOrAddAsync (
			key ,
			addItemFactory: ( cacheEntry ) =>
			{
				cacheEntry.AbsoluteExpirationRelativeToNow = expireSpan;

				return factoryAsync ();
			} );
	}

	public void Remove ( string key )
	{
		_memoryCache.Remove ( key );
	}
}