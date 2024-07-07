namespace TapeCat.Template.Infrastructure.Persistence.Specifications;

using Domain.Contracts.Dtos.QueryDtos;
using System;
using Interfaces;
using DynamicLinqDecorator.Common.Extensions;

public sealed record InlineQuerySpecification : IQuerySpecification
{
    public Func<IQueryable, IQueryable>? QueryInjector { get; private init; }

    public InlineQuerySpecification(
        ExpressionQueryDto? expressionQuery = default,
        OrderQueryDto? orderQuery = default,
        ProjectionQueryDto? projectionQuery = default)
    {
        QueryInjector = query =>
        {
            if (HasExpression(expressionQuery))
                query = query.Where(expressionQuery);

            if (HasOrdering(orderQuery))
                query = query.OrderBy(orderQuery);

            if (HasProjection(projectionQuery))
                query = query.Select(projectionQuery);

            return query;

            static bool HasExpression(ExpressionQueryDto? expressionQuery)
                => !string.IsNullOrEmpty(expressionQuery?.Expression);

            static bool HasOrdering(OrderQueryDto? orderQuery)
                => !string.IsNullOrEmpty(orderQuery?.OrderBy)
                    && orderQuery?.IsDescending is not null;

            static bool HasProjection(ProjectionQueryDto? projectionQuery)
                => !string.IsNullOrEmpty(projectionQuery?.Projection);
        };
    }
}