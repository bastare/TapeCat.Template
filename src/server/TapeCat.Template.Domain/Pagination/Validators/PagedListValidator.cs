namespace TapeCat.Template.Domain.Pagination.Validators;

using FastEndpoints;
using FluentValidation;
using Shared.Interfaces;

public sealed class PagedListValidator : Validator<IPagedList>
{
	public PagedListValidator ()
	{
		RuleFor ( paginationQuery => paginationQuery.CurrentOffset )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );

		RuleFor ( paginationQuery => paginationQuery.Limit )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );

		RuleFor ( paginationQuery => paginationQuery.TotalPages )
			.GreaterThanOrEqualTo ( valueToCompare: 0 );

		RuleFor ( paginationQuery => paginationQuery.TotalCount )
			.GreaterThanOrEqualTo ( valueToCompare: 0 );
	}
}