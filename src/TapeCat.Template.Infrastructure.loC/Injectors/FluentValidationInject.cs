namespace TapeCat.Template.Infrastructure.loC.Injectors;

using FluentValidation;
using FluentValidation.AspNetCore;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class FluentValidationInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection
			.AddFluentValidationAutoValidation ()
			.AddValidatorsFromAssemblyContaining ( type: Assembly.GetEntryAssembly ()!.GetTypes ().First () );
	}
}