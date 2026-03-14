using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class MenuRoleConfiguration : IEntityTypeConfiguration<MenuRole>
{
    public void Configure(EntityTypeBuilder<MenuRole> builder)
    {
        builder.ToTable("MenuRoles", "Identity");
        builder.Ignore(x => x.Id);
        builder.HasKey(e => new { e.MenuId, e.RoleId });
    }
}
