namespace TapeCat.Template.Api.Queries.Validators;

using FastEndpoints;
using FluentValidation;

public sealed class PaginationQueryValidator : Validator<PaginationQuery>
{
    public PaginationQueryValidator()
    {
        RuleFor(paginationQuery => paginationQuery.Limit)
            .GreaterThanOrEqualTo(valueToCompare: 1);

        RuleFor(paginationQuery => paginationQuery.Offset)
            .GreaterThanOrEqualTo(valueToCompare: 1);
    }
}