namespace TapeCat.Template.Domain.Shared.Common.Interfaces;

using Force.DeepCloner;

public abstract class CloneableValueObject : ICloneable
{
	public virtual object Clone ()
		=> this.DeepClone ();
}