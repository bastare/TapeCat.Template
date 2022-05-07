namespace TapeCat.Template.Infrastructure.CrossCutting.Configurators.FluentValidationConfigurators;

using FluentValidation.AspNetCore;

public static class FluentValidationConfigurator
{
	public static void FluentValidationMvcConfigurator ( FluentValidationMvcConfiguration fluentValidationMvcConfiguration )
	{
		fluentValidationMvcConfiguration.RegisterValidatorsFromAssemblies (
			new[]
			{
				Assembly.GetEntryAssembly ()
			} );
	}
}