namespace TapeCat.Template.Infrastructure.loC.Injectors;

using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Caching;
using Domain.Caching.Interfaces;

public sealed class CachingServiceInjector : IInjectable
{
    public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
    {
		serviceCollection.AddResponseCaching ();
        serviceCollection.AddMemoryCache ();
        serviceCollection.AddLazyCache ();
        serviceCollection.AddScoped<ICacheService , LazyMemoryCacheService> ();
    }
}