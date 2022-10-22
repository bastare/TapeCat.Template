namespace TapeCat.Template.Contracts.HomeContracts.Query;

using Common.Attributes;

[RequestClientContract]
public sealed record GetHomeContract ( string? Message );