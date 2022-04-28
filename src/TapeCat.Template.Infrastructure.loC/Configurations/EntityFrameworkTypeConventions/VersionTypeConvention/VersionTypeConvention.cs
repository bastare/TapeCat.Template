namespace TapeCat.Template.Infrastructure.loC.Configurations.EntityFrameworkTypeConventions.VersionTypeConvention;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

public sealed class VersionTypeMapping : RelationalTypeMapping
{
	private static readonly ValueConverter<Version , string> _valueConverter =
		new ( convertToProviderExpression: version => version.ToString () ,
			  convertFromProviderExpression: value => Version.Parse ( value ) );

	private static readonly RelationalTypeMappingParameters _relationalTypeMappingParameters =
		new ( coreParameters: new ( clrType: typeof ( Version ) , _valueConverter ) ,
			  storeType: "char(20)" );

	public VersionTypeMapping ()
		: base ( _relationalTypeMappingParameters )
	{ }

	protected override RelationalTypeMapping Clone ( RelationalTypeMappingParameters _ ) =>
		new VersionTypeMapping ();
}