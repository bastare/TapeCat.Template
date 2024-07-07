namespace TapeCat.Template.Infrastructure.Persistence.Pagination.Extensions;

using Pagination;

public static class PagedListExtensions
{
    public static PagedList<TTransformResult> Select<T, TTransformResult>(this PagedList<T> pagedList, Func<T, TTransformResult> selector)
    {
        NotNullOrEmpty(pagedList);
        NotNull(selector);

        var transformedCollection =
            Enumerable.Select(pagedList, selector);

        return PagedList<TTransformResult>.Create(
            transformedCollection,
            pagedList.TotalCount,
            pagedList.CurrentOffset,
            pagedList.Limit);
    }
}