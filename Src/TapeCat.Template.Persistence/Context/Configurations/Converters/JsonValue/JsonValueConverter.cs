namespace TapeCat.Template.Persistence.Context.Configurations.Converters.JsonValue;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal sealed class JsonValueConverter<TModel> : ValueConverter<TModel? , string>
{
	public JsonValueConverter (
		Expression<Func<TModel? , string>> convertToProviderExpression ,
		Expression<Func<string , TModel?>> convertFromProviderExpression ,
		ConverterMappingHints? mappingHints = null )
			: base (
				convertToProviderExpression ,
				convertFromProviderExpression ,
				mappingHints )
	{ }
}