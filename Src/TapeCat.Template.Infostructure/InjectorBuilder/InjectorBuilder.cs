namespace TapeCat.Template.Infostructure.InjectorBuilder;

using Common.Attributes;
using Common.Interfaces;
using Domain.Shared.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using static Domain.Shared.Helpers.AssertGuard.Guard;

public static class InjectorBuilder
{
	private const string InjectorMethodName = nameof ( IInjectable.Inject );

	public static IServiceCollection CreateDependency ( IServiceCollection serviceCollection ,
														IConfiguration configuration ,
														IEnumerable<Assembly> assemblies )
	{
		NotNull ( serviceCollection , nameof ( serviceCollection ) );
		NotNull ( configuration , nameof ( configuration ) );
		NotNull ( assemblies , nameof ( assemblies ) );

		var injectorTypes = GetAllAssemblyTypes ( assemblies )
			.GetAllInjectorTypes ()
			.OrderByInjectionOrder ();

		return ExecuteInjections ( injectorTypes , ref serviceCollection , configuration );
	}

	private static IEnumerable<Type> GetAllAssemblyTypes ( IEnumerable<Assembly> assemblies )
		=> assemblies
			.Aggregate (
				new HashSet<Type> () ,
				( typeSet , assembly ) =>
				  {
					  var assemblyTypes = assembly.GetTypes ();

					  typeSet.UnionWith ( assemblyTypes );

					  return typeSet;
				  } );

	private static IEnumerable<Type> GetAllInjectorTypes ( this IEnumerable<Type> currentAssemblyTypes )
		=> currentAssemblyTypes
			.Where ( type =>
				type.GetInterfaces ()
					.Contains ( typeof ( IInjectable ) ) );

	private static IEnumerable<Type> OrderByInjectionOrder ( this IEnumerable<Type> injectorTypes )
		=> injectorTypes
			.OrderBy ( ResolveInjectionOrderPosition );

	private static uint ResolveInjectionOrderPosition ( Type injectorType )
		=> injectorType.GetCustomAttribute<InjectionOrderAttribute> ()?.Order ??
			uint.MinValue;

	private static IServiceCollection ExecuteInjections ( IEnumerable<Type> injectorTypes ,
														  ref IServiceCollection serviceCollection ,
														  IConfiguration configuration )
	{
		foreach ( var injectorType in injectorTypes )
		{
			var methodInfo = injectorType.GetMethod ( InjectorMethodName ) ??
				throw new ArgumentNullException (
					nameof ( injectorType ) , $"No method with this name: {InjectorMethodName}" );

			methodInfo.Invoke ( injectorType , serviceCollection , configuration );
		}

		return serviceCollection;
	}
}
