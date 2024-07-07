namespace TapeCat.Template.Infrastructure.loC.Injectors;

using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Caching;
using Domain.Caching.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

public sealed class CachingServiceInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddResponseCaching ();
		serviceCollection.AddMemoryCache ();
		serviceCollection.AddLazyCache ();
		serviceCollection.TryAddScoped<ICacheService , LazyMemoryCacheService> ();
	}
}