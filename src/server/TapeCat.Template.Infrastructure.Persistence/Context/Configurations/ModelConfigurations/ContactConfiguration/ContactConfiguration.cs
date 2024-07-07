namespace TapeCat.Template.Infrastructure.Persistence.Context.Configurations.ModelConfigurations.ContactConfiguration;

using Domain.Core.Models.Contact;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ContactConfiguration : AuditableEntityTypeConfiguration<Contact, int>
{
    public override void Configure(EntityTypeBuilder<Contact> builder)
    {
        base.Configure(builder);
    }
}