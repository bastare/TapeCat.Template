namespace TapeCat.Template.Domain.Core.Models;

public interface IAuditableModel<TKey> : IModel<TKey>, IAuditable<TKey>
    where TKey : struct
{ }