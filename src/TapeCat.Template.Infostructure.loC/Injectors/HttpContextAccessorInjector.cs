namespace TapeCat.Template.Infostructure.loC.Injectors;

using InjectorBuilder.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class HttpContextAccessorInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		serviceCollection.AddTransient<IHttpContextAccessor , HttpContextAccessor> ();
	}
}