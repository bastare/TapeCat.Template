namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using Mapster;
using MongoDB.Driver.Linq;

public static class MapsterExtensions
{
	public static IMongoQueryable<TMappable> ProjectToType<TModel, TMappable> ( this IMongoQueryable<TModel> collection , TypeAdapterConfig? config = null )
	{
		config ??= TypeAdapterConfig.GlobalSettings;

		return collection
			.Select ( @object => @object!.Adapt<TMappable> () );
	}
}