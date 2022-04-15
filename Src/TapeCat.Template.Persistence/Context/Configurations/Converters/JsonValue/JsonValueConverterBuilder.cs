namespace TapeCat.Template.Persistence.Context.Configurations.Converters.JsonValue;

using Domain.Shared.Common.Constants;
using Domain.Shared.Common.Extensions;
using Domain.Shared.Common.Interfaces;
using Newtonsoft.Json;

internal sealed class JsonValueConverterBuilder<TModel> : IBuilder<JsonValueConverter<TModel>>
{
	private Expression<Func<TModel? , string>> _convertToProviderExpression =
		( field ) => JsonConvert.SerializeObject ( field , JsonConversationSettings.EfSerializerSettings );

	private Expression<Func<string , TModel?>> _convertFromProviderExpression =
		( field ) => JsonConvert.DeserializeObject<TModel?> ( field , JsonConversationSettings.EfSerializerSettings );

	public JsonValueConverterBuilder<TModel> AddConvertToProviderExpression ( Expression<Func<TModel? , string>> convertToProviderExpression )
		=> this.Tap ( _ => { _convertToProviderExpression = convertToProviderExpression; } );

	public JsonValueConverterBuilder<TModel> AddConvertFromProviderExpression ( Expression<Func<string , TModel?>> convertFromProviderExpression )
		=> this.Tap ( _ => { _convertFromProviderExpression = convertFromProviderExpression; } );

	public JsonValueConverter<TModel> Build ()
		=> new ( _convertToProviderExpression , _convertFromProviderExpression );
}