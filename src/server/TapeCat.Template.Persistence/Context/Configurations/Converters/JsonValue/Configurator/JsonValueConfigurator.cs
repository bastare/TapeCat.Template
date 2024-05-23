namespace TapeCat.Template.Persistence.Context.Configurations.Converters.JsonValue.Configurator;

using Domain.Core.Models.Common.Attributes;
using Domain.Shared.Common.Extensions;
using Domain.Shared.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal class JsonValueConfigurator (
	ModelBuilder builder ,
	Type modelType ,
	string? jsonFieldTypeName = JsonValueConfigurator.JsonMSSQLFieldTypeName )
{
	protected const string JsonMSSQLFieldTypeName = "nvarchar(max)";

	private readonly ModelBuilder _builder = builder;

	private readonly Type _modelType = modelType;

	private readonly string? _jsonFieldTypeName = jsonFieldTypeName;

	public ModelBuilder Configure ()
	{
		return _builder.Tap ( builder =>
		  {
			  if ( TryResolvePropertiesWithJsonSerializableAttribute ( out var propertiesForJsonConfiguration ) )
			  {
				  foreach ( var propertyForJsonConfiguration in propertiesForJsonConfiguration )
					  ConfigureProperty ( propertyForJsonConfiguration );

				  return builder;
			  }

			  return builder;
		  } )!;

		bool TryResolvePropertiesWithJsonSerializableAttribute ( out IEnumerable<PropertyInfo> propertiesForJsonConfiguration )
		{
			propertiesForJsonConfiguration =
				_modelType.GetProperties ()
					.Where ( HasJsonSerializableAttribute );

			return !propertiesForJsonConfiguration.IsNullOrEmpty ();

			static bool HasJsonSerializableAttribute ( PropertyInfo property )
				=> ResolveJsonSerializableAttribute ( property ) is not null;
		}

		void ConfigureProperty ( PropertyInfo propertyForSerialization )
		{
			var (propertyType, propertyName) =
				(propertyForSerialization.PropertyType, propertyForSerialization.Name);

			var jsonConverter = CreateJsonConverter ( propertyType );

			var isRequiredField =
				ResolveJsonSerializableAttribute ( propertyForSerialization )
					!.IsRequired;

			CreateJsonField ( propertyType , propertyName , jsonConverter , isRequiredField );

			static ValueConverter CreateJsonConverter ( Type serializablePropertyType )
				=> CreateJsonValueConverterBuilder ( serializablePropertyType )
					.Build ();

			static IBuilder<ValueConverter> CreateJsonValueConverterBuilder ( Type serializablePropertyType )
				=> ( Activator.CreateInstance (
					type: typeof ( JsonValueConverterBuilder<> )
						.MakeGenericType ( serializablePropertyType ) )
							as IBuilder<ValueConverter> ) ??
								throw new ArgumentException ( $"Can`t create converter by using this generic: {serializablePropertyType}" );

			void CreateJsonField ( Type propertyType , string propertyName , ValueConverter jsonConverter , bool isRequiredField )
			{
				if ( isRequiredField )
					CreateRequiredJsonField ( propertyType , propertyName , jsonConverter );
				else
					CreateOptionalJsonField ( propertyType , propertyName , jsonConverter );
			}

			void CreateRequiredJsonField ( Type propertyType , string propertyName , ValueConverter jsonConverter )
			{
				CreateJsonFieldInBuilder ( propertyType , propertyName , jsonConverter )
					.IsRequired ();
			}

			void CreateOptionalJsonField ( Type propertyType , string propertyName , ValueConverter jsonConverter )
			{
				CreateJsonFieldInBuilder ( propertyType , propertyName , jsonConverter );
			}

			PropertyBuilder CreateJsonFieldInBuilder ( Type propertyType , string propertyName , ValueConverter jsonConverter )
				=> _builder.Entity ( _modelType )
					.Property ( propertyType , propertyName )
					.HasColumnType ( _jsonFieldTypeName )
					.HasConversion ( jsonConverter );
		}

		static JsonTypeColumnTypeAttribute? ResolveJsonSerializableAttribute ( PropertyInfo property )
			=> property.GetCustomAttribute<JsonTypeColumnTypeAttribute> ();
	}
}

internal sealed class JsonValueConfigurator<TModelEntity> (
	ModelBuilder builder ,
	string? jsonFieldTypeName = JsonValueConfigurator.JsonMSSQLFieldTypeName )
		: JsonValueConfigurator( builder , typeof ( TModelEntity ) , jsonFieldTypeName )
			where TModelEntity : class
{
}