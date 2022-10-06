namespace TapeCat.Template.Infrastructure.loC.Injectors.Common.Extensions;

using Configurations.EntityFrameworkInterceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

public static class DbContextOptionsBuilderExtensions
{
	public static DbContextOptionsBuilder AddInterceptors ( this DbContextOptionsBuilder dbContextOptionsBuilder ,
															IServiceCollection serviceCollection )
	{
		return dbContextOptionsBuilder.AddInterceptors (
			interceptors: InjectInterceptors ( serviceCollection ) );

		static IEnumerable<IInterceptor> InjectInterceptors ( IServiceCollection serviceCollection )
		{
			serviceCollection.Scan ( scan =>
			 {
				 scan.FromAssemblyOf<AuditionInterceptor> ()
					 .AddClasses ( classes => { classes.AssignableTo<IInterceptor> (); } )
						 .AsImplementedInterfaces ()

						 .WithScopedLifetime ();
			 } );

			return ResolveInterceptors ( serviceCollection );

			static IEnumerable<IInterceptor> ResolveInterceptors ( IServiceCollection serviceCollection )
				=> serviceCollection.BuildServiceProvider ()
					.GetRequiredService<IEnumerable<IInterceptor>> ();
		}
	}
}