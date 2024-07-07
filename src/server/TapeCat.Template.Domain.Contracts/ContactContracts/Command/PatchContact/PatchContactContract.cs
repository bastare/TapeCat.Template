namespace TapeCat.Template.Domain.Contracts.ContactContracts.Command.PatchContact;

using Common.Attributes;
using Dtos;

[RequestClientContract]
public sealed record PatchContactContract ( ContactForPatchDto ContactForPatch );