namespace TapeCat.Template.Infostructure.InjectorBuilder;

using Common.Attributes;
using Common.Interfaces;
using Domain.Shared.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;

public static class InjectorBuilder
{
	private const string InjectorMethodName = nameof ( IInjectable.Inject );

	public static IServiceCollection CreateDependency ( IServiceCollection serviceCollection ,
														IConfiguration configuration ,
														IEnumerable<Assembly> assemblies )
	{
		NotNull ( serviceCollection );
		NotNull ( configuration );
		NotNullOrEmpty ( assemblies );

		return ExecuteInjections (
			ref serviceCollection ,
			injectorTypes: ResolveInjectorTypes ( assemblies ) ,
			configuration );

		static IEnumerable<Type> ResolveInjectorTypes ( IEnumerable<Assembly> assemblies )
			=> GetAllAssemblyTypes ( assemblies )
				.Where ( IsInjectorType )
				.OrderBy ( InjectionOrder );

		static IEnumerable<Type> GetAllAssemblyTypes ( IEnumerable<Assembly> assemblies )
			=> assemblies
				.Aggregate (
					new HashSet<Type> () ,
					( typeSet , assembly ) =>
			 		  {
						   typeSet.UnionWith (
							   other: ResolveAssemblyTypes ( assembly ) );

						   return typeSet;

						   static IEnumerable<Type> ResolveAssemblyTypes ( Assembly assembly )
							   => assembly.GetTypes ();
					   } );

		static bool IsInjectorType ( Type type )
			=> type.GetInterfaces ()
				.Contains ( typeof ( IInjectable ) );

		static uint InjectionOrder ( Type injectorType )
			=> injectorType.GetCustomAttribute<InjectionOrderAttribute> ()?.Order ??
				uint.MinValue;

		static IServiceCollection ExecuteInjections ( ref IServiceCollection serviceCollection ,
													  IEnumerable<Type> injectorTypes ,
													  IConfiguration configuration )
		{
			foreach ( var injectorType in injectorTypes )
				ResolveInjectionMethod ( injectorType )
					.Invoke ( injectorType , serviceCollection , configuration );

			return serviceCollection;

			static MethodInfo ResolveInjectionMethod ( Type injectorType )
				=> injectorType.GetMethod ( InjectorMethodName ) ??
					throw new ArgumentNullException ( nameof ( injectorType ) , $"No method with this name: {InjectorMethodName}" );
		}
	}
}