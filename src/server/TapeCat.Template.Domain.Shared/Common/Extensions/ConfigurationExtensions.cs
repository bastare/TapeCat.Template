namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
	public static string GetRequiredSectionValue ( this IConfiguration configuration , string? key )
	{
		NotNullOrEmpty ( key );

		return configuration.GetSection ( key! )?.Value ??
			throw new ArgumentNullException ( nameof ( key ) , $"No section with dis `{key}`, in `appSettings` file" );
	}

	public static IConfigurationSection GetRequiredSection ( this IConfiguration configuration , string? key )
	{
		NotNullOrEmpty ( key );

		return configuration.GetSection ( key! ) ??
			throw new ArgumentNullException ( nameof ( key ) , $"No section with dis `{key}`, in `appSettings` file" );
	}
}