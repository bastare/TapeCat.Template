namespace TapeCat.Template.Infostructure.loC.Injectors;

using Domain.Shared.Authorization.Session;
using Domain.Shared.Authorization.Session.Interfaces;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[InjectionOrder ( order: 1 )]
public sealed class SessionInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddTransient<IUserSession , UserSession> ();
	}
}