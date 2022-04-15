namespace TapeCat.Template.Api.Caching.Intrefaces;

using System;
using System.Threading;
using System.Threading.Tasks;

public interface ICacheService
{
	Task<TCachedValue> GetAsync<TCachedValue> ( string key , CancellationToken cancellationToken = default );

	Task SetAsync<TCachedValue> (
		string key , TCachedValue value , TimeSpan expiriSpan , CancellationToken cancellationToken = default );

	bool TryGet<TCachedValue> ( string key , out TCachedValue value );

	Task<TCachedValue> GetOrCreateCacheValueAsync<TCachedValue> (
		string key , TCachedValue value , TimeSpan expiriSpan , CancellationToken cancellationToken = default );

	void Remove ( string key );
}