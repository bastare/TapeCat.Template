namespace TapeCat.Template.Api.Queries.Interfaces;

public interface IOrderQuery
{
    bool? IsDescending { get; }

    string? OrderBy { get; }
}