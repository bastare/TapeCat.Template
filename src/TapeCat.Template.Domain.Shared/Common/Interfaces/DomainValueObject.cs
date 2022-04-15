namespace TapeCat.Template.Domain.Shared.Common.Interfaces;

using Attributes;
using Equ;

public abstract class DomainValueObject<TSelf> : CloneableValueObject, IEquatable<TSelf>
	where TSelf : DomainValueObject<TSelf>
{
	private IEnumerable<PropertyInfo>? _properties;

	private IEnumerable<FieldInfo>? _fields;

	private IEnumerable<PropertyInfo> Properties =>
		_properties ??= GetProperties ();

	private IEnumerable<FieldInfo> Fields =>
		_fields ??= GetFields ();

	private IEnumerable<PropertyInfo> GetProperties ()
		=> GetType ().GetProperties ( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic )
			.Where ( HasNoIgnoreMemberAttribute );

	private IEnumerable<FieldInfo> GetFields ()
		=> GetType ().GetFields ( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic )
			.Where ( HasNoIgnoreMemberAttribute );

	private static bool HasNoIgnoreMemberAttribute ( MemberInfo memberInfo )
		=> !Attribute.IsDefined ( memberInfo , typeof ( IgnoreMemberAttribute ) );

	public static bool operator == ( DomainValueObject<TSelf>? @object , DomainValueObject<TSelf>? otherObject )
		=> AreObjectsEqual ( @object , otherObject );

	public static bool operator != ( DomainValueObject<TSelf>? @object , DomainValueObject<TSelf>? otherObject )
		=> !AreObjectsEqual ( @object , otherObject );

	private static bool AreObjectsEqual ( object? @object , object? otherObject )
	{
		if ( AreObjectsNull ( @object , otherObject ) )
			return true;

		return @object!.Equals ( otherObject );
	}

	private static bool AreObjectsNull ( object? @object , object? otherObject )
		=> @object is null
			&& otherObject is null;

	public override bool Equals ( object? other )
		=> Equals ( other! as TSelf );

	public bool Equals ( TSelf? other )
		=> EquCompare<TSelf>.Equals ( ( TSelf ) this , other! );

	public override int GetHashCode ()
		=> Enumerable.Concat ( ResolvePropertiesValues () , ResolveFieldsValues () )
			.Where ( value => value is not null )

			.Aggregate (
				new HashCode () ,
				( hashCodeBuilder , @object ) =>
				  {
					  hashCodeBuilder.Add ( @object );

					  return hashCodeBuilder;
				  } )

			.ToHashCode ();

	private IEnumerable<object?> ResolvePropertiesValues ()
		=> Properties
			.Select ( propertyInfo => propertyInfo.GetValue ( this , default ) );

	private IEnumerable<object?> ResolveFieldsValues ()
		=> Fields
			.Select ( fieldInfo => fieldInfo.GetValue ( this ) );
}