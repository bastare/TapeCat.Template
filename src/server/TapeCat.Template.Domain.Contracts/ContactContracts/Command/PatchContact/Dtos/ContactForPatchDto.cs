namespace TapeCat.Template.Domain.Contracts.ContactContracts.Command.PatchContact.Dtos;

public sealed record ContactForPatchDto
{
	public int Id { get; set; }

	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? Email { get; set; }

	public string? Phone { get; set; }

	public string? Title { get; set; }

	public string? MiddleInitial { get; set; }
}