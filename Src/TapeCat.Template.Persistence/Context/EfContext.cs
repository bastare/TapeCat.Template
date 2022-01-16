namespace TapeCat.Template.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using System.Reflection;

public sealed class EfContext : DbContext
{
	public EfContext ( DbContextOptions<EfContext> options )
		: base ( options )
	{ }

	protected override void OnModelCreating ( ModelBuilder modelBuilder )
	{
		modelBuilder.ApplyConfigurationsFromAssembly ( Assembly.GetExecutingAssembly () );
	}
}