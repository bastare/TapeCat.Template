namespace TapeCat.Template.Domain.Contracts.ContactContracts.Command.CreateContact.Dtos;

public sealed record ContactForCreationDto
{
	public string? FirstName { get; init; }

	public string? LastName { get; init; }

	public string? Email { get; init; }

	public string? Phone { get; init; }

	public string? Title { get; init; }

	public string? MiddleInitial { get; init; }
}