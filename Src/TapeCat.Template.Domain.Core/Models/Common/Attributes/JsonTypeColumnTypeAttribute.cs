namespace TapeCat.Template.Domain.Core.Models.Common.Attributes;

/// <summary>
/// Decorate domain model, to create json field in db
/// </summary>
[AttributeUsage ( AttributeTargets.Property , AllowMultiple = false )]
public sealed class JsonTypeColumnTypeAttribute : Attribute
{
	public bool IsRequired { get; set; }
}