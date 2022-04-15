namespace TapeCat.Template.Persistence.Common.Extensions;

using Context.Configurations.Converters.JsonValue.Configurator;
using Microsoft.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
	public static ModelBuilder ConfigureJsonSerelizableFields<TModelEntity> ( this ModelBuilder builder , string? jsonFieldTypeName )
		where TModelEntity : class
			=> new JsonValueConfigurator<TModelEntity> ( builder , jsonFieldTypeName )
				.Configure ();

	public static ModelBuilder ConfigureJsonSerelizableFields ( this ModelBuilder builder , Type modelType , string? jsonFieldTypeName )
		=> new JsonValueConfigurator ( builder , modelType , jsonFieldTypeName )
			.Configure ();
}