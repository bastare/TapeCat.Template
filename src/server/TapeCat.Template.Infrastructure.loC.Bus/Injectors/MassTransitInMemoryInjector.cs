namespace TapeCat.Template.Infrastructure.loC.Bus.Injectors;

using Application.Brokers.Home.Consumers.Query;
using Contracts.HomeContracts.Query;
using Infrastructure.loC.Bus.Configurations.Filters;
using Infrastructure.loC.Bus.Injectors.Common.Extensions;
using InjectorBuilder.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class MassTransitInMemoryInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddMediator ( mediatorRegistrationConfigurator =>
		  {
			  mediatorRegistrationConfigurator.AddConsumers (
				  assemblies:
				  [
					  typeof ( GetHomeConsumer ).Assembly
				  ] );

			  mediatorRegistrationConfigurator.AddRequestClient (
				  assembliesWithRequestClients:
				  [
					  typeof ( GetHomeContract ).Assembly
				  ] );

			  mediatorRegistrationConfigurator.ConfigureMediator ( ( mediatorRegistrationContext , mediatorConfigurator ) =>
				{
					mediatorConfigurator.UseConsumeFilter ( typeof ( ValidationFilter<> ) , mediatorRegistrationContext );
				} );
		  } );
	}
}