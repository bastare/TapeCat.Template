namespace TapeCat.Template.Domain.Shared.Common.Interfaces;

using Force.DeepCloner;
using System;

public abstract class CloneableValueObject : ICloneable
{
	public object Clone ()
		=> this.DeepClone ();
}