namespace TapeCat.Template.Api.Common.Extensions;

using Caching;
using Caching.Intrefaces;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static void AddCaching ( this IServiceCollection serviceCollection )
	{
		serviceCollection.AddMemoryCache ();

		serviceCollection.AddInMemoryCacheService ();
	}

	private static void AddInMemoryCacheService ( this IServiceCollection serviceCollection )
	{
		serviceCollection.AddSingleton<ICacheService , InMemoryCacheService> ();
	}
}
