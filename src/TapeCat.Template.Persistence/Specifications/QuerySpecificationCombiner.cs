namespace TapeCat.Template.Persistence.Specifications;

using Domain.Core.Models;
using System.Linq;

//TODO: Finish me
public static class QuerySpecificationCombiner
{
	public static TSpecification Combine<TSpecification, TModel, TKey> ( params TSpecification[] specifications )
		where TSpecification : QuerySpecification<TModel , TKey>, new()
		where TModel : class, IModel<TKey>
			=> specifications
				.Aggregate (
					new TSpecification () ,
					( newSpecification , specification ) =>
					  {
						  return newSpecification;
					  } );
}