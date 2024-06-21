namespace TapeCat.Template.Infrastructure.loC.Injectors.PersistenceServicesInjectors;

using Configurations.EntityFrameworkInterceptors.AuditionTriggers;
using InjectorBuilder.Common.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.Uow;
using Persistence.Uow.Interfaces;

[InjectionOrder ( order: uint.MaxValue )]
public sealed class EfInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		var sqliteConnection_ = CreateAndPersistSqlConnection ( serviceCollection );

		serviceCollection.AddDbContext<EfContext> (
			optionsAction: ( dbContextOptionsBuilder ) =>
			{
				dbContextOptionsBuilder
					.UseLoggerFactory ( loggerFactory: ResolveLoggerFactory ( serviceCollection ) )

					.UseSqlite ( sqliteConnection_ )

					.UseTriggers ( triggerOptions =>
						triggerOptions.AddTrigger<OnAuditionTrigger> () );
			} );

		serviceCollection.TryAddScoped<IUnitOfWork<int> , EfUnitOfWork<EfContext , int>> ();
		serviceCollection.TryAddScoped<IEfUnitOfWork<EfContext , int> , EfUnitOfWork<EfContext , int>> ();
		serviceCollection.TryAddScoped<ITransaction , EfUnitOfWork<EfContext , int>> ();

		// TODO: Don't forget to create post-operation life-hook for injectors
		EnsureCreated ( serviceCollection );

		static SqliteConnection CreateAndPersistSqlConnection ( IServiceCollection serviceCollection )
		{
			var sqliteConnection_ = new SqliteConnection ( "DataSource=:memory:" );
			sqliteConnection_.Open ();

			serviceCollection.TryAddSingleton ( sqliteConnection_ );

			return sqliteConnection_;
		}

		static ILoggerFactory ResolveLoggerFactory ( IServiceCollection serviceCollection )
			=> serviceCollection.BuildServiceProvider ()
				.GetRequiredService<ILoggerFactory> ();

		static void EnsureCreated ( IServiceCollection serviceCollection )
		{
			serviceCollection.BuildServiceProvider ()
				.GetRequiredService<IEfUnitOfWork<EfContext , int>> ()
					.EnsureCreated ();
		}
	}
}