namespace TapeCat.Template.Infrastructure.Persistence.Context.Configurations.ModelConfigurations;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class AuditableEntityTypeConfiguration<TAuditableEntity, TKey> : ModelEntityTypeConfiguration<TAuditableEntity , TKey>
	where TAuditableEntity : class, IAuditableModel<TKey>
	where TKey : struct
{
	public override void Configure ( EntityTypeBuilder<TAuditableEntity> builder )
	{
		ConfigureAuditableFields ();

		base.Configure ( builder );

		void ConfigureAuditableFields ()
		{
			builder.Property ( auditableModel => auditableModel.CreatedBy )
				.HasColumnType ( "uniqueidentifier" );

			builder.Property ( auditableModel => auditableModel.Created )
				.HasColumnType ( "datetime2" )
				.HasDefaultValueSql ( "GETDATE()" )
				.IsRequired ();

			builder.Property ( auditableModel => auditableModel.LastModifiedBy )
				.HasColumnType ( "uniqueidentifier" );

			builder.Property ( auditableModel => auditableModel.LastModified )
				.HasColumnType ( "datetime2" );
		}
	}
}