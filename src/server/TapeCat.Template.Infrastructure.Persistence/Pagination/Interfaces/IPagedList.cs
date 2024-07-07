namespace TapeCat.Template.Infrastructure.Persistence.Pagination.Interfaces;

public interface IPagedList : IEnumerable
{
    ulong CurrentOffset { get; }

    ulong TotalPages { get; }

    ulong Limit { get; }

    ulong TotalCount { get; }
}

public interface IPagedList<out T> : IPagedList, IEnumerable<T>
{ }