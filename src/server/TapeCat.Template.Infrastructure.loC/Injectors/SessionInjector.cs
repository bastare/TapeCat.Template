namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Domain.Shared.Authorization.Session;
using Domain.Shared.Authorization.Session.Interfaces;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[InjectionOrder ( order: 1 )]
public sealed class SessionInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.TryAddTransient<IUserSession , UserSession> ();
	}
}