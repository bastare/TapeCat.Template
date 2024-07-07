namespace TapeCat.Template.Domain.Contracts.ContactContracts.Query.GetContacts;

using Dtos.Decorators.Interfaces;

public sealed record SubmitContactsContract ( IPaginationRowsDecoratorDto ContactsForQueryResponse );