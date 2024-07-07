namespace TapeCat.Template.Infrastructure.Persistence.Common.Extensions;

using TapeCat.Template.Infrastructure.Persistence.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
	public static IQueryable<TModel> Set<TModel> ( this DbContext dbContext , bool isTracking = default )
		where TModel : class
			=> isTracking
				? dbContext.Set<TModel> ()

				: dbContext.Set<TModel> ()
					.AsNoTracking ();

	public static async Task CommitAsync ( this DbContext dbContext , CancellationToken cancellationToken = default )
	{
		if ( await TryCommitAsync ( dbContext , cancellationToken ) )
			return;

		throw new DataWasNotSavedException ();
	}

	public static async Task<bool> TryCommitAsync ( this DbContext dbContext , CancellationToken cancellationToken = default )
		=> await dbContext.SaveChangesAsync ( cancellationToken ) != 0;
}