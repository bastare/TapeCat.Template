namespace TapeCat.Template.Infrastructure.Bus.Brokers.Contact.Consumers.Command;

using MassTransit;
using Mapster;
using System.Threading.Tasks;
using Domain.Contracts;
using Persistence.Context;
using Domain.Core.Models.Contact;
using Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;
using Domain.Contracts.ContactContracts.Command.PatchContact;
using Domain.Shared.Common.Exceptions;
using Persistence.Common.Extensions;

public sealed class PatchContactConsumer(EfContext efContext) :
    IConsumer<PatchContactContract>
{
    private readonly EfContext _efContext = efContext;

    public async Task Consume(ConsumeContext<PatchContactContract> context)
    {
        try
        {
            var patchedContact_ =
                await PatchContactAsync(context.Message.ContactForPatch);

            await _efContext.TryCommitAsync(context.CancellationToken);

            await context.RespondAsync<SubmitPatchedContactsContract>(
                new(ContactFromPatch: patchedContact_.Adapt<ContactFromPatchDto>()));
        }
        catch (Exception exception)
        {
            await context.RespondAsync<FaultContract>(
                new(exception));
        }

        async Task<Contact> PatchContactAsync(ContactForPatchDto contactForPatch)
        {
            // TODO: Integrate `ExecuteUpdate`
            var config = new TypeAdapterConfig();
            config.Default.IgnoreNullValues(true);

            var contactForUpdate_ =
                await _efContext.Contacts.FindAsync(contactForPatch.Id, context.CancellationToken) ??
                    throw new NotFoundException(message: $"There is no `Contact` with this id: {contactForPatch.Id}");

            return contactForPatch.Adapt(contactForUpdate_, config)!;
        }
    }
}