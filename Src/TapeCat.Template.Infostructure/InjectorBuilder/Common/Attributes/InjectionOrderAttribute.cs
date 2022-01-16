namespace TapeCat.Template.Infostructure.InjectorBuilder.Common.Attributes;

using System;

[AttributeUsage ( AttributeTargets.Class , AllowMultiple = false , Inherited = false )]
public sealed class InjectionOrderAttribute : Attribute
{
	public uint Order { get; set; }
}