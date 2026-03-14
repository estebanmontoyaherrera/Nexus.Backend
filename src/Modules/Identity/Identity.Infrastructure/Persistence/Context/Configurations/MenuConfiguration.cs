using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus", "Identity");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("MenuId");

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsUnicode(false);

        builder.Property(e => e.Icon)
                .HasMaxLength(50);

        builder.Property(e => e.Url)
            .HasMaxLength(150);
    }
}
