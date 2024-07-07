namespace TapeCat.Template.Infrastructure.loC.Configurations.EntityFrameworkTriggers.AuditionTriggers;

using Domain.Core.Models;
using Domain.Shared.Authorization.Session.Interfaces;
using EntityFrameworkCore.Triggered;

public sealed class OnAuditionTrigger(IUserSession session) : IBeforeSaveTrigger<IAuditable>
{
    private readonly IUserSession _session = session;

    public Task BeforeSave(ITriggerContext<IAuditable> context, CancellationToken cancellationToken)
    {
        return _session.IsAuthorizedUser()
            ? InsertUserAuditionData()
            : InsertAuditionData();

        Task InsertUserAuditionData()
        {
            switch (context.ChangeType)
            {
                case ChangeType.Added:
                    context.Entity.CreatedBy = _session.Id;
                    context.Entity.Created = DateTime.UtcNow;

                    break;

                case ChangeType.Modified:
                    context.Entity.LastModifiedBy = _session.Id!;
                    context.Entity.LastModified = DateTime.UtcNow;

                    break;
            }

            return Task.CompletedTask;
        }

        Task InsertAuditionData()
        {
            switch (context.ChangeType)
            {
                case ChangeType.Added:
                    context.Entity.Created = DateTime.UtcNow;

                    break;

                case ChangeType.Modified:
                    context.Entity.LastModified = DateTime.UtcNow;

                    break;
            }

            return Task.CompletedTask;
        }
    }
}