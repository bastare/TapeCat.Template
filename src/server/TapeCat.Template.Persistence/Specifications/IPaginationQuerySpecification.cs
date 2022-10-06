namespace TapeCat.Template.Persistence.Specifications;

using Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

public interface IPaginationQuerySpecification<TModel, TKey>
	where TModel : IModel<TKey>
{
	[Range ( 1 , int.MaxValue )]
	int Limit { get; }

	[Range ( 1 , int.MaxValue )]
	int Offset { get; }
}