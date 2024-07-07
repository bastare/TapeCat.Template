namespace TapeCat.Template.Domain.Core.Models;

using System;

public interface IAuditable<TKey> : IAuditable
    where TKey : struct
{
    new TKey CreatedBy { get; set; }

    new TKey? LastModifiedBy { get; set; }
}

public interface IAuditable
{
    object CreatedBy { get; set; }

    DateTime Created { get; set; }

    object? LastModifiedBy { get; set; }

    DateTime? LastModified { get; set; }
}