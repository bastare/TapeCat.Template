namespace TapeCat.Template.Infrastructure.Bus.Brokers.Contact.Consumers.Query;

using MassTransit;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Contracts.ContactContracts.Query.GetContacts;
using Mapster;
using Domain.Contracts.Dtos.WrapDtos.Interfaces;
using Persistence.Context;
using Persistence.Pagination.Interfaces;
using Persistence.Specifications.Evaluator.Common.Extensions;
using Persistence.Specifications;
using Persistence.Common.Extensions;

public sealed class GetContactsConsumer(EfContext efContext) :
    IConsumer<GetContactsContract>
{
    private readonly EfContext _efContext = efContext;

    public async Task Consume(ConsumeContext<GetContactsContract> context)
    {
        try
        {
            var contacts_ = await GetContactsAsync(context.Message);

            await context.RespondAsync<SubmitContactsContract>(
                new(ContactsForQueryResponse: contacts_.Adapt<IPaginationRowsDto>()));
        }
        catch (Exception exception)
        {
            await context.RespondAsync<FaultContract>(
                new(exception));
        }

        async Task<IPagedList> GetContactsAsync(GetContactsContract getContactsContract)
            => await _efContext.Contacts
                .SpecifiedQuery(
                    inlineSpecification: new InlineQuerySpecification(
                        getContactsContract.ExpressionQuery,
                        getContactsContract.OrderQuery,
                        getContactsContract.ProjectionQuery))

                .ToPagedListAsync(
                    getContactsContract.PaginationQuery!.Offset,
                    getContactsContract.PaginationQuery!.Limit,
                    context.CancellationToken);
    }
}