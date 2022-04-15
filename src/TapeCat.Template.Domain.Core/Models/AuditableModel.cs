namespace TapeCat.Template.Domain.Core.Models;

using Shared.Common.Interfaces;
using System;

public abstract class AuditableModel<TSelf, TKey> : DomainValueObject<TSelf>, IModel<TKey>, IAuditable<TKey>
	where TSelf : DomainValueObject<TSelf>
	where TKey : struct
{
	public TKey Id { get; set; }

	public TKey CreatedBy { get; set; }

	public DateTime Created { get; set; }

	public TKey? LastModifiedBy { get; set; }

	public DateTime? LastModified { get; set; }
}