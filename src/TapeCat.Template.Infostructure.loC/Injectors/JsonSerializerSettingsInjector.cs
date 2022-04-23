namespace TapeCat.Template.Infostructure.loC.Injectors;

using Domain.Shared.Common.Constants;
using Domain.Shared.Common.Extensions;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

public sealed class JsonSerializerSettingsInjector : IInjectable
{
	public void Inject ( IServiceCollection serviceCollection , IConfiguration configuration )
	{
		JsonConversationSettings.DefaultSerializerSettings
			.Tap ( jsonSerializerSettings =>
			  {
				  JsonConvert.DefaultSettings =
					() => jsonSerializerSettings;

				  serviceCollection.AddSingleton ( jsonSerializerSettings );
			  } );
	}
}