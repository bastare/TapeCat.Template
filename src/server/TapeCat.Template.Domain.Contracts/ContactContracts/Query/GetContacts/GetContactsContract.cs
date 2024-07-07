namespace TapeCat.Template.Domain.Contracts.ContactContracts.Query.GetContacts;

using Common.Attributes;
using Domain.Contracts.Dtos.QueryDtos;

[RequestClientContract]
public sealed record GetContactsContract(
    ExpressionQueryDto? ExpressionQuery,
    OrderQueryDto? OrderQuery,
    PaginationQueryDto? PaginationQuery,
    ProjectionQueryDto? ProjectionQuery);