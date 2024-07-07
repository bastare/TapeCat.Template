namespace TapeCat.Template.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using Mapster;
using System.Threading.Tasks;
using Domain.Contracts;
using Infrastructure.Persistence.Context;
using Domain.Core.Models.Contact;
using Domain.Contracts.ContactContracts.Command.CreateContact.Dtos;
using Domain.Contracts.ContactContracts.Command.CreateContact;

public sealed class CreateContactConsumer ( EfContext efContext ) :
	IConsumer<CreateContactContract>
{
	private readonly EfContext _efContext = efContext;

	public async Task Consume ( ConsumeContext<CreateContactContract> context )
	{
		try
		{
			var createdContact_ =
				await CreateContactsAsync ( context.Message.ContactForCreation );

			await _efContext.SaveChangesAsync ( context.CancellationToken );

			await context.RespondAsync<SubmitCreatedContactContract> (
				new ( CreatedContact: createdContact_.Adapt<ContactFromCreationDto> () ) );
		}
		catch ( Exception exception )
		{
			await context.RespondAsync<FaultContract> (
				new ( exception ) );
		}

		async Task<Contact> CreateContactsAsync ( ContactForCreationDto contactForCreation )
		{
			var modelContactForCreation_ = contactForCreation.Adapt<Contact> ();

			await _efContext.Contacts
				.AddAsync (
					modelContactForCreation_ ,
					context.CancellationToken );

			return modelContactForCreation_;
		}
	}
}