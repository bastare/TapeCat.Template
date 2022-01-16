namespace TapeCat.Template.Persistence.Context.Configurations.ModelConfigurations;

using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

public abstract class AuditableEntityTypeConfiguration<TAuditableEntity, TKey> : ModelEntityTypeConfiguration<TAuditableEntity , TKey>
	where TAuditableEntity : AuditableModel<TAuditableEntity , TKey>
	where TKey : struct
{
	private const string AdminUserGuid = "fe4ab75d-d646-4d6e-853e-75321077c885";

	public override void Configure ( EntityTypeBuilder<TAuditableEntity> builder )
	{
		ConfigureAuditableFields ( ref builder );

		base.Configure ( builder );
	}

	private static void ConfigureAuditableFields ( ref EntityTypeBuilder<TAuditableEntity> builder )
	{
		builder.Property ( auditableModel => auditableModel.CreatedBy )
			.HasColumnType ( "uniqueidentifier" )
			.HasDefaultValue ( Guid.Parse ( AdminUserGuid ) )
			.IsRequired ();

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
