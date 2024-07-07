namespace TapeCat.Template.Infrastructure.Persistence.Specifications.Evaluator.Common.Extensions;

using Domain.Core.Models;

public static class QueryableExtensions
{
    public static IQueryable SpecifiedQuery(this IQueryable inputQuery, InlineQuerySpecification inlineSpecification)
    {
        NotNull(inputQuery);
        NotNull(inlineSpecification?.QueryInjector);

        return inlineSpecification!.QueryInjector!.Invoke(inputQuery);
    }

    public static IQueryable SpecifiedQuery<TModel, TKey>(this IQueryable<TModel> inputQuery, InlineQuerySpecification inlineSpecification)
        where TModel : class, IModel<TKey>
    {
        NotNull(inputQuery);
        NotNull(inlineSpecification);

        return SpecifiedQuery(inputQuery, inlineSpecification);
    }

    public static IQueryable<TModel> SpecifiedQuery<TModel, TKey>(this IQueryable<TModel> inputQuery, QuerySpecification<TModel, TKey> querySpecification)
         where TModel : class, IModel<TKey>
    {
        NotNull(inputQuery);
        NotNull(querySpecification?.QueryInjector);

        return querySpecification!.QueryInjector!.Invoke(inputQuery);
    }
}