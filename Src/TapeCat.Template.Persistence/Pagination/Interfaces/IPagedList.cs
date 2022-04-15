namespace TapeCat.Template.Persistence.Pagination.Interfaces;

public interface IPagedList<out T> : IEnumerable<T>
{
	int CurrentOffset { get; }

	int TotalPages { get; }

	int Limit { get; }

	int TotalCount { get; }
}