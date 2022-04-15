namespace TapeCat.Template.Domain.Shared.Common.Attributes;

using MongoDB.Bson.Serialization.Attributes;

[AttributeUsage ( AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct )]
public sealed class BsonGeneralDiscriminatorAttribute : BsonDiscriminatorAttribute { }