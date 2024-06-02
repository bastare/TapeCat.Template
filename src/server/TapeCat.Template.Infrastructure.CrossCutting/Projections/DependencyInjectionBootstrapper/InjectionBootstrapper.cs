namespace TapeCat.Template.Infrastructure.CrossCutting.Projections.DependencyInjectionBootstrapper;

using Autofac;
using InjectorBuilder;
using loC.Bus.Injectors;
using loC.Injectors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InjectionBootstrapper
{
	private static readonly Assembly[] AssembliesForScanning = [
		typeof ( ErrorHandlerInjector ).Assembly,
		typeof ( MassTransitConfigurationInjector ).Assembly
	];

	public static IServiceCollection InjectLayersDependency ( this IServiceCollection serviceCollection , IConfiguration configuration )
	{
		NotNull ( serviceCollection );
		NotNull ( configuration );

		return InjectorBuilder.CreateDependency (
			serviceCollection ,
			configuration ,
			assemblies: AssembliesForScanning );
	}

	public static void InjectLayersDependency ( this ContainerBuilder containerBuilder )
	{
		NotNull ( containerBuilder );

		containerBuilder.RegisterAssemblyOpenGenericTypes ( AssembliesForScanning );
		containerBuilder.RegisterAssemblyModules ( AssembliesForScanning );
	}
}