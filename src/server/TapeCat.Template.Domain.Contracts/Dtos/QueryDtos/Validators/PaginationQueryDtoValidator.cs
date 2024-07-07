namespace TapeCat.Template.Domain.Contracts.Dtos.QueryDtos.Validators;

using FastEndpoints;
using FluentValidation;

public sealed class PaginationQueryDtoValidator : Validator<PaginationQueryDto>
{
	public PaginationQueryDtoValidator ()
	{
		RuleFor ( paginationQuery => paginationQuery.Limit )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );

		RuleFor ( paginationQuery => paginationQuery.Offset )
			.GreaterThanOrEqualTo ( valueToCompare: 1 );
	}
}