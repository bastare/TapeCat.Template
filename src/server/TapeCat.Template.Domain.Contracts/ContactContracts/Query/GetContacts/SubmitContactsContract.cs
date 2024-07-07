namespace TapeCat.Template.Domain.Contracts.ContactContracts.Query.GetContacts;

using Domain.Contracts.Dtos.WrapDtos.Interfaces;

public sealed record SubmitContactsContract ( IPaginationRowsDto ContactsForQueryResponse );