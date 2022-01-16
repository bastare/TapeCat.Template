namespace TapeCat.Template.Domain.Core.Models;

using System;

public interface IAuditable<TKey>
	where TKey : struct
{
	TKey CreatedBy { get; set; }

	DateTime Created { get; set; }

	TKey? LastModifiedBy { get; set; }

	DateTime? LastModified { get; set; }
}