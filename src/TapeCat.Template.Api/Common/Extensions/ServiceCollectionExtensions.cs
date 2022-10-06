namespace TapeCat.Template.Api.Common.Extensions;

using Api.Caching.Interfaces;
using Caching;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCaching ( this IServiceCollection serviceCollection )
	{
		serviceCollection.AddMemoryCache ();

		serviceCollection.AddInMemoryCacheService ();

		return serviceCollection;
	}

	private static IServiceCollection AddInMemoryCacheService ( this IServiceCollection serviceCollection )
	{
		serviceCollection.AddSingleton<ICacheService , InMemoryCacheService> ();

		return serviceCollection;
	}
}
