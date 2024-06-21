namespace TapeCat.Template.Infrastructure.loC.Injectors.PersistenceServicesInjectors;

using Configurations.EntityFrameworkInterceptors.AuditionTriggers;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.Uow;
using Persistence.Uow.Interfaces;
using System;

[InjectionOrder ( order: uint.MaxValue )]
public sealed class EfInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		serviceCollection.AddDbContext<EfContext> (
			optionsAction: ( dbContextOptionsBuilder ) =>
			{
				dbContextOptionsBuilder
					.UseLoggerFactory ( loggerFactory: ResolveLoggerFactory ( serviceCollection ) )

					.UseSqlServer (
						connectionString: configuration.GetConnectionString ( string.Empty ) ,
						sqlServerOptionsAction: ( sqlServerDbContextOptionsBuilder ) =>
						{
							sqlServerDbContextOptionsBuilder
								.MigrationsAssembly ( typeof ( EfContext ).Assembly.FullName )
								.UseQuerySplittingBehavior ( QuerySplittingBehavior.SplitQuery );
						} )

					.UseTriggers ( triggerOptions =>
						triggerOptions.AddTrigger<OnAuditionTrigger> () );
			} );

		serviceCollection.TryAddScoped<IUnitOfWork<Guid> , EfUnitOfWork<EfContext , Guid>> ();
		serviceCollection.TryAddScoped<IEfUnitOfWork<EfContext , Guid> , EfUnitOfWork<EfContext , Guid>> ();
		serviceCollection.TryAddScoped<ITransaction , EfUnitOfWork<EfContext , Guid>> ();

		static ILoggerFactory ResolveLoggerFactory ( IServiceCollection serviceCollection )
			=> serviceCollection.BuildServiceProvider ()
				.GetRequiredService<ILoggerFactory> ();
	}
}