namespace TapeCat.Template.Persistence.Context;

using AgileObjects.NetStandardPolyfills;
using Configurations.ConfigurationBootstrapper;
using Configurations.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

public sealed class EfContext (
	DbContextOptions<EfContext> options ,
	ModelCreatingConfigurator modelCreatingConfigurator )
		: DbContext( options )
{
	private readonly ModelCreatingConfigurator _modelCreatingConfigurator = modelCreatingConfigurator;

	protected override void OnModelCreating ( ModelBuilder modelBuilder )
	{
		ApplyConfigurationsFromAssembly ( modelBuilder );
		ApplyConfigurationsFromConfigurator ( modelBuilder );

		static void ApplyConfigurationsFromAssembly ( ModelBuilder modelBuilder )
		{
			modelBuilder.ApplyConfigurationsFromAssembly ( assembly: GetAssemblyWithConfigurations () );

			static Assembly GetAssemblyWithConfigurations ()
				=> typeof ( ModelEntityTypeConfiguration<,> )
					.GetAssembly ();
		}

		void ApplyConfigurationsFromConfigurator ( ModelBuilder modelBuilder )
		{
			_modelCreatingConfigurator.Configure ( modelBuilder );
		}
	}
}