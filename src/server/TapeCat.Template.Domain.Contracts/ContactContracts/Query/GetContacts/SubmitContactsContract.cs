namespace TapeCat.Template.Domain.Contracts.ContactContracts.Query.GetContacts;

using Dtos.WrapDtos.Interfaces;

public sealed record SubmitContactsContract ( IPaginationRowsDto ContactsForQueryResponse );