namespace TapeCat.Template.Infrastructure.loC.Injectors.MapperBootstrap.Configurations;

using Domain.Contracts.Dtos.WrapDtos.Interfaces;
using Mapster;
using Persistence.Pagination.Interfaces;

public sealed class PaginationConfiguration : IRegister
{
	public void Register ( TypeAdapterConfig config )
	{
		config.NewConfig<IPagedList , IPaginationRowsDto> ()
			.Map (
				paginationRows => paginationRows.Rows ,
				pagedList => pagedList );
	}
}