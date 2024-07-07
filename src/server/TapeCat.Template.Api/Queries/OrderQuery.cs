namespace TapeCat.Template.Api.Queries;

using Interfaces;

public sealed record OrderQuery : IOrderQuery
{
    public bool? IsDescending { get; init; }

    public string? OrderBy { get; init; }
}