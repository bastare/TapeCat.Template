namespace TapeCat.Template.Api.Configurations.RouteEndpointConfiguration;

using Common.Attributes;
using Domain.Shared.Common.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

public sealed class PluralFormResourceNameConvention : IControllerModelConvention
{
	public void Apply ( ControllerModel controller )
	{
		if ( HasPluralFormResourceNameAttribute ( controller ) )
			PluralizeControllerName ( ref controller );
	}

	private static bool HasPluralFormResourceNameAttribute ( ControllerModel controller )
		=> controller.Attributes.OfType<PluralFormResourceNameAttribute> ().Any ();

	private static void PluralizeControllerName ( ref ControllerModel controller )
	{
		controller.ControllerName = controller.ControllerName.ToPluralForm ();
	}
}
