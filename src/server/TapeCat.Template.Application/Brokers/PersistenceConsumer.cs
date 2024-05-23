namespace TapeCat.Template.Application.Brokers;

using MassTransit;
using Persistence.Uow.Interfaces;
using System.Threading.Tasks;

public abstract class PersistenceConsumer<TMessage, TTransaction> ( TTransaction transaction ) : IConsumer<TMessage>
	where TMessage : class
	where TTransaction : ITransaction
{
	protected TTransaction Transaction { get; } = transaction;

	public abstract Task Consume ( ConsumeContext<TMessage> context );
}
