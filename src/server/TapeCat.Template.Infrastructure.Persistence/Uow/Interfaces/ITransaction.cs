namespace TapeCat.Template.Infrastructure.Persistence.Uow.Interfaces;

public interface ITransaction
{
	Task<bool> TryCommitAsync ( CancellationToken cancellationToken = default );

	Task CommitAsync ( CancellationToken cancellationToken = default );

	Task<int> SaveChangesAsync ( CancellationToken cancellationToken = default );
}