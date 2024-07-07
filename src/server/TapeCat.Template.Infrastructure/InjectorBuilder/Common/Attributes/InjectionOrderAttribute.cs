namespace TapeCat.Template.Infrastructure.InjectorBuilder.Common.Attributes;

using System;

[AttributeUsage ( AttributeTargets.Class , AllowMultiple = false , Inherited = false )]
public sealed class InjectionOrderAttribute ( uint order ) : Attribute
{
	public uint Order { get; } = order;
}