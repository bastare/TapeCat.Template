namespace TapeCat.Template.Infrastructure.Persistence.Context.Configurations.Converters.JsonValue;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal sealed class JsonValueConverter<TModel> (
	Expression<Func<TModel? , string>> convertToProviderExpression ,
	Expression<Func<string , TModel?>> convertFromProviderExpression ,
	ConverterMappingHints? mappingHints = null ) :
		ValueConverter<TModel? , string>(
			convertToProviderExpression ,
			convertFromProviderExpression ,
			mappingHints )
{
}