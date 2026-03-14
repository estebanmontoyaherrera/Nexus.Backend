using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles", "Identity");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("RoleId");

        builder.Property(e => e.Name)
            .HasMaxLength(50);

        builder.Property(e => e.Description)
                .HasMaxLength(255);
    }
}
