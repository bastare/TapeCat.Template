namespace TapeCat.Template.Domain.Contracts.ContactContracts.Command.CreateContact;

using Common.Attributes;
using Dtos;

[RequestClientContract]
public sealed record CreateContactContract(ContactForCreationDto ContactForCreation);