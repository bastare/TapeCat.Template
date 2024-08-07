namespace TapeCat.Template.Infrastructure.loC.Injectors;

using FluentValidation;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Contracts.Dtos.QueryDtos;

public sealed class FluentValidationInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration _ )
	{
		serviceCollection.AddValidatorsFromAssemblies (
			assemblies: [
				Assembly.GetEntryAssembly ()!,
				typeof ( ExpressionQueryDto ).Assembly
			] );
	}
}