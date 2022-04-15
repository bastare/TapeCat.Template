namespace TapeCat.Template.Infostructure.CrossCutting.Projections.DependencyInjectionBootstrapper;

using Autofac;
using InjectorBuilder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InjectionBootstrapper
{
	private const string ParentNamespaceRoot = nameof ( TapeCat );

	private static Assembly[] AssembliesForScanning { get; } =
		Assembly.GetExecutingAssembly ()
			.GetReferencedAssemblies ()
				.Where ( assemblyName =>
					assemblyName.Name?.StartsWith ( ParentNamespaceRoot ) ?? false )

				.Select ( Assembly.Load )

				.ToArray ();

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