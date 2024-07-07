namespace TapeCat.Template.Domain.Contracts.ContactContracts.Command.CreateContact;

using Dtos;

public sealed record SubmitCreatedContactContract(ContactFromCreationDto CreatedContact);