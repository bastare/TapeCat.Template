namespace TapeCat.Template.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using System.Threading.Tasks;
using Domain.Contracts;
using Persistence.Context;
using Domain.Contracts.ContactContracts.Command.RemoveContact;
using Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

public sealed class RemoveContactConsumer ( EfContext efContext ) :
	IConsumer<RemoveContactContract>
{
	private readonly EfContext _efContext = efContext;

	public async Task Consume ( ConsumeContext<RemoveContactContract> context )
	{
		try
		{
			await RemoveContactsAsync ( context.Message.Id );

			await _efContext.TryCommitAsync ( context.CancellationToken );

			await context.RespondAsync<SubmitRemovedContactsContract> ( new () );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		Task RemoveContactsAsync ( int contactIdForRemove )
			=> _efContext.Contacts
				.Where ( contact => contact.Id == contactIdForRemove )
				.ExecuteDeleteAsync ( context.CancellationToken );
	}
}