namespace TapeCat.Template.Infrastructure.Persistence.Pagination;

using Interfaces;

public sealed record PagedList<T> : IPagedList<T>
{
    private readonly IReadOnlyList<T> _immutablePagedList;

    public T? this[int index] => _immutablePagedList[index];

    public ulong CurrentOffset { get; private set; }

    public ulong TotalPages { get; private set; }

    public ulong Limit { get; private set; }

    public ulong TotalCount { get; private set; }

    public IEnumerator<T> GetEnumerator()
        => _immutablePagedList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private PagedList(IEnumerable<T> items)
        => _immutablePagedList = [.. items];

    public static PagedList<T> Create(IEnumerable<T> items, ulong count, ulong offset, ulong limit)
    {
        NotNull(items);
        ParametersAreValid(limit);

        return new(items)
        {
            CurrentOffset = offset,
            TotalPages = (ulong)CalculateTotalPages(count, limit),
            Limit = limit,
            TotalCount = count
        };

        static void ParametersAreValid(ulong limit)
        {
            if (limit <= 0)
                throw new ArgumentException($"{nameof(limit)}: {limit}, has the `zero` or negative value");
        }

        static double CalculateTotalPages(ulong count, ulong limit)
            => Math.Ceiling(count / (double)limit);
    }
}