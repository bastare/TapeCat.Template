namespace TapeCat.Template.Api.Endpoints.v1.Contact.Contracts;

public sealed record ContactForPatchRequestBody
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public string? Title { get; init; }

    public string? MiddleInitial { get; init; }
}