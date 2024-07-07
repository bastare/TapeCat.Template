namespace TapeCat.Template.Infrastructure.loC.Injectors;

using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class HttpContextAccessorInjector : IInjectable
{
    public void Inject(IServiceCollection serviceCollection, IConfiguration _)
    {
        serviceCollection.AddHttpContextAccessor();
    }
}