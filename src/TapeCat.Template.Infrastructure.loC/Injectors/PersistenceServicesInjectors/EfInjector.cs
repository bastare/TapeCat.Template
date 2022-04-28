namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Common.Extensions;
using Configurations.EntityFrameworkTypeConventions.VersionTypeConvention;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.Uow;
using Persistence.Uow.Interfaces;
using System;
using Thinktecture;

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

					  .AddInterceptors ( serviceCollection )

					  .AddRelationalTypeMappingSourcePlugin<VersionTypeMappingPlugin> ();
			  } );

		serviceCollection.AddScoped<IUnitOfWork<Guid> , EfUnitOfWork<EfContext , Guid>> ();
		serviceCollection.AddScoped<IEfUnitOfWork<EfContext , Guid> , EfUnitOfWork<EfContext , Guid>> ();
		serviceCollection.AddScoped<ITransaction , EfUnitOfWork<EfContext , Guid>> ();

		static ILoggerFactory ResolveLoggerFactory ( IServiceCollection serviceCollection )
			=> serviceCollection.BuildServiceProvider ()
				.GetRequiredService<ILoggerFactory> ();
	}
}