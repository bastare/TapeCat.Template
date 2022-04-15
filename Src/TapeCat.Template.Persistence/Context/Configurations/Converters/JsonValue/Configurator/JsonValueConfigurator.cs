namespace TapeCat.Template.Persistence.Context.Configurations.Converters.JsonValue.Configurator;

using Domain.Core.Models.Common.Attributes;
using Domain.Shared.Common.Extensions;
using Domain.Shared.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal class JsonValueConfigurator
{
	protected const string JsonMSSQLFieldTypeName = "nvarchar(max)";

	private readonly ModelBuilder _builder;

	private readonly Type _modelType;

	private readonly string? _jsonFieldTypeName;

	public JsonValueConfigurator (
		ModelBuilder builder ,
		Type modelType ,
		string? jsonFieldTypeName = JsonMSSQLFieldTypeName )
	{
		_builder = builder;
		_jsonFieldTypeName = jsonFieldTypeName;
		_modelType = modelType;
	}

	public ModelBuilder Configure ()
	{
		return _builder.Tap ( builder =>
		  {
			  if ( TryResolvePropertiesWithJsonSerelizableAttribute ( out var propertiesForJsonConfiguration ) )
			  {
				  foreach ( var propertyForJsonConfiguration in propertiesForJsonConfiguration )
					  ConfigureProperty ( propertyForJsonConfiguration );

				  return builder;
			  }

			  return builder;
		  } );

		bool TryResolvePropertiesWithJsonSerelizableAttribute ( out IEnumerable<PropertyInfo> propertiesForJsonConfiguration )
		{
			propertiesForJsonConfiguration =
				_modelType.GetProperties ()
					.Where ( HasJsonSerelizableAttribute );

			return !propertiesForJsonConfiguration.IsNullOrEmpty ();

			static bool HasJsonSerelizableAttribute ( PropertyInfo property )
				=> ResolveJsonSerelizableAttribute ( property ) is not null;
		}

		void ConfigureProperty ( PropertyInfo propertyForSerialization )
		{
			var (propertyType, propertyName) =
				(propertyForSerialization.PropertyType, propertyForSerialization.Name);

			var jsonConverter = CreateJsonConverter ( propertyType );

			var isRequiredField =
				ResolveJsonSerelizableAttribute ( propertyForSerialization )
					!.IsRequired;

			CreateJsonField ( propertyType , propertyName , jsonConverter , isRequiredField );

			static ValueConverter CreateJsonConverter ( Type serelizablePropertyType )
				=> CreateJsonValueConverterBuilder ( serelizablePropertyType )
					.Build ();

			static IBuilder<ValueConverter> CreateJsonValueConverterBuilder ( Type serelizablePropertyType )
				=> ( Activator.CreateInstance (
					type: typeof ( JsonValueConverterBuilder<> )
						.MakeGenericType ( serelizablePropertyType ) )
							as IBuilder<ValueConverter> ) ??
								throw new ArgumentException ( $"Can`t create converter by using this generic: {serelizablePropertyType}" );

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

		static JsonTypeColumnTypeAttribute? ResolveJsonSerelizableAttribute ( PropertyInfo property )
			=> property.GetCustomAttribute<JsonTypeColumnTypeAttribute> ();
	}
}

internal sealed class JsonValueConfigurator<TModelEntity> : JsonValueConfigurator
	where TModelEntity : class
{
	public JsonValueConfigurator ( ModelBuilder builder , string? jsonFieldTypeName = JsonMSSQLFieldTypeName )
		: base ( builder , typeof ( TModelEntity ) , jsonFieldTypeName )
	{ }
}