namespace TapeCat.Template.Infrastructure.Persistence.Context;

using AgileObjects.NetStandardPolyfills;
using Configurations.ModelConfigurations;
using TapeCat.Template.Domain.Core.Models.Contact;
using Microsoft.EntityFrameworkCore;

public sealed class EfContext ( DbContextOptions<EfContext> options ) :
	DbContext ( options )
{
	public DbSet<Contact> Contacts { get; set; }

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