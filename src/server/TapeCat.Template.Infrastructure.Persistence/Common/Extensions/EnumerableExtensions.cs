namespace TapeCat.Template.Infrastructure.Persistence.Common.Extensions;

using Pagination;

public static class EnumerableExtensions
{
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> collection, ulong count, ulong offset, ulong limit)
        => PagedList<T>.Create(collection, count, offset, limit);
}