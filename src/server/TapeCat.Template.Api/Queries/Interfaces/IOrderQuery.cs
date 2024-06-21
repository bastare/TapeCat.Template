namespace TapeCat.Template.Api.Queries.Interfaces;

using Microsoft.AspNetCore.Mvc;

public interface IOrderQuery
{
	[FromQuery ( Name = "isDescending" )]
	bool? IsDescending { get; }

	string? OrderBy { get; }
}