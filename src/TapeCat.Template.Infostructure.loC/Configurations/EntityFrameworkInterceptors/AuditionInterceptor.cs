namespace TapeCat.Template.Infostructure.loC.Configurations.EntityFrameworkInterceptors;

using Domain.Core.Models;
using Domain.Shared.Authorization.Session;
using Fluentx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence.Context;

public sealed class AuditionInterceptor : ISaveChangesInterceptor
{
	private readonly UserSession _userSession;

	public AuditionInterceptor ( UserSession userSession )
	{
		_userSession = userSession;
	}

	public void SaveChangesFailed ( DbContextErrorEventData _ ) { }

	public Task SaveChangesFailedAsync ( DbContextErrorEventData _ , CancellationToken __ = default )
		=> Task.CompletedTask;

	public int SavedChanges ( SaveChangesCompletedEventData _ , int result )
		=> result;

	public ValueTask<int> SavedChangesAsync ( SaveChangesCompletedEventData _ , int result , CancellationToken __ = default )
		=> ValueTask.FromResult ( result );

	public InterceptionResult<int> SavingChanges ( DbContextEventData _ , InterceptionResult<int> result )
		=> result;

	public ValueTask<InterceptionResult<int>> SavingChangesAsync ( DbContextEventData dbContextEventData ,
																   InterceptionResult<int> result ,
																   CancellationToken _ = default )
	{
		if ( TryResolveEfContext ( dbContextEventData , out var efContext ) && IsAuthorizedContext () )
			efContext!.ChangeTracker.Entries<IAuditable<Guid>> ()
				.ForEach ( UpdateAuditableFields );

		return ValueTask.FromResult ( result );

		static bool TryResolveEfContext ( DbContextEventData dbContextEventData , out EfContext? efContext )
		{
			if ( dbContextEventData.Context is EfContext context )
			{
				efContext = context;
				return true;
			}

			efContext = default;
			return false;
		}

		bool IsAuthorizedContext ()
		 	=> _userSession.IsAuthorizedUser ();

		void UpdateAuditableFields ( EntityEntry<IAuditable<Guid>> entry )
		{
			switch ( entry.State )
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = _userSession.Id!.Value;
					entry.Entity.Created = DateTime.Now;

					break;

				case EntityState.Modified:
					entry.Entity.LastModifiedBy = _userSession.Id!.Value;
					entry.Entity.LastModified = DateTime.Now;

					break;
			}
		}
	}
}