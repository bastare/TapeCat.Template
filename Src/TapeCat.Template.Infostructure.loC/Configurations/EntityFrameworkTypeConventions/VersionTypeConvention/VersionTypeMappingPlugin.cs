namespace TapeCat.Template.Infostructure.loC.Configurations.EntityFrameworkTypeConventions.VersionTypeConvention;

using Microsoft.EntityFrameworkCore.Storage;
using System;

public sealed class VersionTypeMappingPlugin : IRelationalTypeMappingSourcePlugin
{
	public RelationalTypeMapping? FindMapping ( in RelationalTypeMappingInfo mappingInfo ) =>
		mappingInfo.ClrType == typeof ( Version )
			? new VersionTypeMapping ()
			: default;
}