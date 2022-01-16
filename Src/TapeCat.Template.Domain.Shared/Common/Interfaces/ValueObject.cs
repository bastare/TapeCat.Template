namespace TapeCat.Template.Domain.Shared.Common.Interfaces;

using Equ;
using System;

public abstract class ValueObject<TSelf> : CloneableValueObject, IEquatable<TSelf>
	where TSelf : ValueObject<TSelf>
{
	public static bool operator == ( ValueObject<TSelf> @object , ValueObject<TSelf> otherObject )
		=> AreObjectsEqual ( @object , otherObject );

	public static bool operator != ( ValueObject<TSelf> @object , ValueObject<TSelf> otherObject )
		=> !AreObjectsEqual ( @object , otherObject );

	private static bool AreObjectsEqual ( object @object , object otherObject )
	{
		if ( AreObjectsNullable ( @object , otherObject ) )
			return true;

		return @object.Equals ( otherObject );
	}

	private static bool AreObjectsNullable ( object @object , object otherObject )
		=> @object is null
			&& otherObject is null;

	public override bool Equals ( object? other )
		=> Equals ( ( TSelf ) other! );

	public bool Equals ( TSelf? other )
		=> EquCompare<TSelf>.Equals ( ( TSelf ) this , other! );

	public override int GetHashCode ()
		=> EquCompare<TSelf>.GetHashCode ( ( TSelf ) this );
}