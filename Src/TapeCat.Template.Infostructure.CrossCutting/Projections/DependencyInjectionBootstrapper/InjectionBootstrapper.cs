namespace TapeCat.Template.Infostructure.CrossCutting.Projections.DependencyInjectionBootstrapper;

using Autofac;
using InjectorBuilder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using static Domain.Shared.Helpers.AssertGuard.Guard;

public static class InjectionBootstrapper
{
	private const string ParentNamespaceRoot = nameof ( TapeCat.Template );

	private static Assembly[] AssembliesForScanning { get; } =
		Assembly.GetExecutingAssembly ()
			.GetReferencedAssemblies ()
				.Where ( assemblyName =>
					assemblyName.Name?.StartsWith ( ParentNamespaceRoot ) ?? false )

				.Select ( assemblyName => Assembly.Load ( assemblyName ) )

				.ToArray ();

	public static IServiceCollection InjectLayersDependency ( this IServiceCollection serviceCollection , IConfiguration configuration )
	{
		NotNull ( serviceCollection , nameof ( serviceCollection ) );
		NotNull ( configuration , nameof ( configuration ) );

		return InjectorBuilder.CreateDependency (
			serviceCollection ,
			configuration ,
			assemblies: AssembliesForScanning );
	}

	public static void InjectLayersDependency ( this ContainerBuilder containerBuilder )
	{
		NotNull ( containerBuilder , nameof ( containerBuilder ) );

		containerBuilder.RegisterAssemblyOpenGenericTypes ( AssembliesForScanning );
		containerBuilder.RegisterAssemblyModules ( AssembliesForScanning );
	}
}