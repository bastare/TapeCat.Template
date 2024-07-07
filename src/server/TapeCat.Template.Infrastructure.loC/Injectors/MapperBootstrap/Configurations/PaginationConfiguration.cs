namespace TapeCat.Template.Infrastructure.loC.Injectors.MapperBootstrap.Configurations;

using Mapster;
using Domain.Contracts.Dtos.Decorators.Interfaces;
using Domain.Shared.Interfaces;

public sealed class PaginationConfiguration : IRegister
{
    public void Register ( TypeAdapterConfig config )
    {
        config.NewConfig<IPagedList , IPaginationRowsDecoratorDto> ()
            .Map (
                paginationRows => paginationRows.Rows ,
                pagedList => pagedList );
    }
}