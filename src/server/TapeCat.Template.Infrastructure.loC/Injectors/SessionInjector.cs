namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Autofac;
using Domain.Shared.Authorization.Session.Interfaces;

public sealed class SessionInjector : Module
{
	protected override void Load ( ContainerBuilder builder )
	{
		InjectAllApplicationFacades ( builder );

		static void InjectAllApplicationFacades ( ContainerBuilder builder )
			=> builder.RegisterAssemblyTypes ( typeof ( IUserSession ).Assembly )
				.AsImplementedInterfaces ()
				.AsSelf ()
				.InstancePerLifetimeScope ();
	}
}