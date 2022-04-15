namespace TapeCat.Template.Persistence.Context.Configurations.ModelConfigurations;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class ModelEntityTypeConfiguration<TModelEntity, TKey> : IEntityTypeConfiguration<TModelEntity>
	where TModelEntity : class, IModel<TKey>
	where TKey : struct
{
	public virtual void Configure ( EntityTypeBuilder<TModelEntity> builder )
	{
		ConfigureMSSQLPrimaryKey ();

		void ConfigureMSSQLPrimaryKey ()
		{
			builder.Property ( model => model.Id )
				.HasDefaultValueSql ( "NEWID()" )
				.IsRequired ();
		}
	}
}