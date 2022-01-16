namespace TapeCat.Template.Persistence.Uow.Interfaces;

using System.Threading;
using System.Threading.Tasks;

public interface ITransaction
{
	Task<bool> TryCommitAsync ( CancellationToken cancellationToken = default );

	Task CommitAsync ( CancellationToken cancellationToken = default );
}