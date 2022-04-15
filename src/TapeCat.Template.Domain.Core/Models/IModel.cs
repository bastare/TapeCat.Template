namespace TapeCat.Template.Domain.Core.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public interface IModel<TKey>
{
	[Key, DatabaseGenerated ( DatabaseGeneratedOption.Identity )]
	TKey Id { get; set; }
}