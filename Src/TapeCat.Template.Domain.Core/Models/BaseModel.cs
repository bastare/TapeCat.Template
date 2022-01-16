namespace TapeCat.Template.Domain.Core.Models;

using Shared.Common.Interfaces;

public abstract class BaseModel<TSelf, TKey> : DomainValueObject<TSelf>, IModel<TKey>
	where TSelf : DomainValueObject<TSelf>
	where TKey : struct
{
	public TKey Id { get; set; }
}