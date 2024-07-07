namespace TapeCat.Template.Infrastructure.InjectorBuilder;

using Common.Attributes;
using Common.Interfaces;
using Domain.Shared.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;

public static class InjectorBuilder
{
	private const string InjectorMethodName = nameof ( IInjectable.Inject );

	private const string IsInjectableMethodName = nameof ( IInjectable.IsInjectable );

	public static IServiceCollection CreateDependency ( IServiceCollection serviceCollection ,
														IConfiguration configuration ,
														IEnumerable<Assembly> assemblies )
	{
		NotNull ( serviceCollection );
		NotNull ( configuration );
		NotNullOrEmpty ( assemblies );

		return ExecuteInjections (
			serviceCollection ,
			injectorTypes: ResolveInjectorTypes ( assemblies ) ,
			configuration );

		static IEnumerable<Type> ResolveInjectorTypes ( IEnumerable<Assembly> assemblies )
		{
			return assemblies
				.SelectMany ( GetAllAssemblyTypes )
				.Where ( IsInjectorType )
				.OrderBy ( InjectionOrder );

			static IEnumerable<Type> GetAllAssemblyTypes ( Assembly assembly )
				=> assembly.GetTypes ();

			static bool IsInjectorType ( Type type )
				=> type.GetInterfaces ()
					.Contains ( typeof ( IInjectable ) );

			static uint InjectionOrder ( Type injectorType )
				=> injectorType.GetCustomAttribute<InjectionOrderAttribute> ()?.Order ??
					uint.MinValue;
		}

		static IServiceCollection ExecuteInjections ( IServiceCollection serviceCollection ,
													  IEnumerable<Type> injectorTypes ,
													  IConfiguration configuration )
		{
			injectorTypes
				.Where ( injectorType => IsInjectable ( injectorType , serviceCollection , configuration ) )
				.ForEach ( injectorType => Inject ( injectorType , serviceCollection , configuration ) );

			return serviceCollection;

			static bool IsInjectable ( Type injectorType , IServiceCollection serviceCollection , IConfiguration configuration )
			{
				return ( bool ) ResolveIsInjectableMethod ()
					.Invoke ( injectorType , serviceCollection , configuration )!;

				static MethodInfo ResolveIsInjectableMethod ()
					=> typeof ( IInjectable ).GetMethod ( IsInjectableMethodName ) ??
						throw new ArgumentException ( $"No method with this name: {IsInjectableMethodName}" );
			}

			static void Inject ( Type injectorType , IServiceCollection serviceCollection , IConfiguration configuration )
			{
				ResolveInjectionMethod ( injectorType )
					.Invoke ( injectorType , serviceCollection , configuration );

				static MethodInfo ResolveInjectionMethod ( Type injectorType )
					=> injectorType.GetMethod ( InjectorMethodName ) ??
						throw new ArgumentException ( $"No method with this name: {InjectorMethodName}" );
			}
		}
	}
}