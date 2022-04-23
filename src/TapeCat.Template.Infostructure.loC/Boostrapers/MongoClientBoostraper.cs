namespace TapeCat.Template.Infostructure.loC.Boostrapers;

using MongoDB.Bson.Serialization;
using System.Text.RegularExpressions;

public static class MongoClientBoostraper
{
	private const string DtoEntityKeyword = "Dto";

	public static void BoostrapLookupSerializerByDtoEntities ( Assembly[] assemblies )
	{
		foreach ( var typeOfDtoEntity in GetTypesOfDtoEntities ( assemblies ) )
			BsonSerializer.LookupSerializer ( typeOfDtoEntity );

		static HashSet<Type> GetTypesOfDtoEntities ( Assembly[] assemblies )
			=> assemblies.Aggregate (
				new HashSet<Type> () ,
				( typesOfDtoEntities , assemblyOfDtoEntities ) =>
				  {
					  typesOfDtoEntities.UnionWith (
						other: ResolveDtoTypes ( assemblyOfDtoEntities ) );

					  return typesOfDtoEntities;

					  static IEnumerable<Type> ResolveDtoTypes ( Assembly assemblyOfDtoEntities )
						  => assemblyOfDtoEntities.GetTypes ()
							  .Where ( type =>
								  Regex.IsMatch (
									  input: type.Name ,
									  pattern: string.Concat ( DtoEntityKeyword , "$" ) ) );
				  } );
	}
}