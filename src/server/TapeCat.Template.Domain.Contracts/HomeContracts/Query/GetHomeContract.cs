namespace TapeCat.Template.Domain.Contracts.HomeContracts.Query;

using Common.Attributes;

[RequestClientContract]
public sealed record GetHomeContract ( string? Message );