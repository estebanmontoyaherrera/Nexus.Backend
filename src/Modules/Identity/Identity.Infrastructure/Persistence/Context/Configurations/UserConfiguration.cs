using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "Identity");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("UserId");

        builder.Property(e => e.FirstName)
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .HasMaxLength(100);

        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}
