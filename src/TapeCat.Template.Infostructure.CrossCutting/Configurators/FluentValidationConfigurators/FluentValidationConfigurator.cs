namespace TapeCat.Template.Infostructure.CrossCutting.Configurators.FluentValidationConfigurators;

using FluentValidation.AspNetCore;
using System.Reflection;

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