namespace TapeCat.Template.Domain.Contracts.ContactContracts.Command.RemoveContact;

using Common.Attributes;

[RequestClientContract]
public sealed record RemoveContactContract(int Id);