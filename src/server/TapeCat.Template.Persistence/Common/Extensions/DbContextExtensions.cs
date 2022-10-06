namespace TapeCat.Template.Persistence.Common.Extensions;

using Microsoft.EntityFrameworkCore;

public static class DbContextExtensions
{
	public static IQueryable<TModel> Set<TModel> ( this DbContext dbContext , bool isTracking = default )
		where TModel : class
			=> isTracking
				? dbContext.Set<TModel> ()

				: dbContext.Set<TModel> ()
					.AsNoTracking ();
}