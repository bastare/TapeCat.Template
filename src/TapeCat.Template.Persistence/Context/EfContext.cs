namespace TapeCat.Template.Persistence.Context;

using AgileObjects.NetStandardPolyfills;
using Configurations.ConfigurationBootstraper;
using Configurations.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

public sealed class EfContext : DbContext
{
	private readonly ModelCreatingConfigurator _modelCreatingConfigurator;

	public EfContext (
		DbContextOptions<EfContext> options ,
		ModelCreatingConfigurator modelCreatingConfigurator )
			: base ( options )
	{
		_modelCreatingConfigurator = modelCreatingConfigurator;
	}

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