namespace TapeCat.Template.Domain.Contracts.Dtos.QueryDtos;

public sealed record PaginationQueryDto ( long Offset = 1 , long Limit = 100 );