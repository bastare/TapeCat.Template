namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Autofac;
using Domain.Shared.Authorization.Session.Interfaces;
using Domain.Shared.Authorization.Session;

public sealed class SessionInjector : Module
{
	protected override void Load ( ContainerBuilder builder )
	{
		builder.RegisterGeneric ( typeof ( UserSession<> ) )
			.As ( typeof ( IUserSession<> ) )
			.InstancePerLifetimeScope ();

		// ? Change id to ur entity
		builder.Register ( context => context.Resolve<IUserSession<object>> () )
			.As<IUserSession> ()
			.InstancePerLifetimeScope ();
	}
}