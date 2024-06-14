namespace TapeCat.Template.Domain.Contracts.Dtos.QueryDtos;

using System.ComponentModel.DataAnnotations;

public sealed record PaginationQueryDto (
	[Range ( 1 , int.MaxValue )] int Offset = 1 ,
	[Range ( 1 , int.MaxValue )] int Limit = 100 );