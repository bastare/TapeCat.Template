namespace TapeCat.Template.Infrastructure.Persistence.Context;

using AgileObjects.NetStandardPolyfills;
using Configurations.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

public sealed class EfContext (
	DbContextOptions<EfContext> options )
		: DbContext( options )
{

	protected override void OnModelCreating ( ModelBuilder modelBuilder )
	{
		ApplyConfigurationsFromAssembly ( modelBuilder );

		static void ApplyConfigurationsFromAssembly ( ModelBuilder modelBuilder )
		{
			modelBuilder.ApplyConfigurationsFromAssembly ( assembly: GetAssemblyWithConfigurations () );

			static Assembly GetAssemblyWithConfigurations ()
				=> typeof ( ModelEntityTypeConfiguration<,> )
					.GetAssembly ();
		}
	}
}