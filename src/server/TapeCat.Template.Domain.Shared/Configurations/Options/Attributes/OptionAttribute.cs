namespace TapeCat.Template.Domain.Shared.Configurations.Options.Attributes;

[AttributeUsage ( AttributeTargets.Class , AllowMultiple = false )]
public sealed class OptionAttribute : Attribute
{
	public string SectionName { get; }

	public OptionAttribute ( string? sectionName )
	{
		NotNullOrEmpty ( sectionName );

		SectionName = sectionName!;
	}
}