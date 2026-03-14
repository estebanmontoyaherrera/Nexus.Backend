using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions", "Identity");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("PermissionId");

        builder.Property(e => e.Name)
            .HasMaxLength(100);

        builder.Property(e => e.Description)
                .HasMaxLength(255);
    }
}
