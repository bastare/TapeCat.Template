namespace TapeCat.Template.Infrastructure.loC.Bus.Injectors;

using Castle.Core.Internal;
using Domain.Shared.Configurations.Options;
using Domain.Shared.Configurations.Options.Attributes;
using InjectorBuilder.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

public sealed class MassTransitRebbitMqBusInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddMassTransit ( massTransitConfigure =>
		{
			AddRequestClients ( massTransitConfigure );

			AddConsumers ( massTransitConfigure );

			massTransitConfigure.UsingRabbitMq ( ( contextAgentExtensions , rabbitMqBusFactoryConfigurator ) =>
			  {
				  ConfigureRebbitMqHost ( contextAgentExtensions , rabbitMqBusFactoryConfigurator );

				  rabbitMqBusFactoryConfigurator.ConfigureEndpoints ( contextAgentExtensions , KebabCaseEndpointNameFormatter.Instance );
			  } );

			static void AddConsumers ( IBusRegistrationConfigurator _ )
			{
			}

			static void AddRequestClients ( IBusRegistrationConfigurator _ )
			{
			}

			static void ConfigureRebbitMqHost ( IBusRegistrationContext contextAgentExtensions , IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator )
			{
				var rabbitMqOption = contextAgentExtensions.GetRequiredService<IOptions<RabbitMqOption>> ()
					.Value;

				rabbitMqBusFactoryConfigurator.Host (
					rabbitMqOption.Host ,
					rabbitMqOption.Port ,
					rabbitMqOption.VirtualHost ,
					configure: ( rabbitMqHostConfigurator ) =>
					  {
						  rabbitMqHostConfigurator.Username ( rabbitMqOption.Username! );
						  rabbitMqHostConfigurator.Password ( rabbitMqOption.Password! );

						  rabbitMqHostConfigurator.UseSsl ( configureSsl =>
							{
								configureSsl.Protocol = SslProtocols.Tls12;
							} );
					  } );
			}
		} );
	}

	public bool IsInjectable ( IServiceCollection _ , IConfiguration configuration )
		=> configuration.GetSection (
			key: typeof ( RabbitMqOption ).GetAttribute<OptionAttribute> ()
				.SectionName )
					?.Value is not null;
}