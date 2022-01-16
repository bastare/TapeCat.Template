namespace TapeCat.Template.Domain.Shared.Common.Attributes;

using System;

[AttributeUsage ( AttributeTargets.Property | AttributeTargets.Field )]
public sealed class IgnoreMemberAttribute : Attribute { }