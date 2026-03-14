using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Tests.Fixtures;

namespace Identity.Tests.IntegrationTests.Persistence;

public class MenuAndPermissionRepositoryIntegrationTests
{
    [Fact]
    public async Task GetMenuByUserIdAsync_WhenRelationshipExists_ReturnsMenu()
    {
        await using var context = InMemoryDbContextFactory.Create();

        var role = new Role { Name = "Admin", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        var user = new User { FirstName = "U", LastName = "T", Email = "u@test.com", Password = "x", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        var menu = new Menu { Name = "Dashboard", Position = 1, Url = "/dash", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };

        context.AddRange(role, user, menu);
        await context.SaveChangesAsync();

        context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        context.MenuRoles.Add(new MenuRole { RoleId = role.Id, MenuId = menu.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var repo = new MenuRepository(context);
        var result = await repo.GetMenuByUserIdAsync(user.Id);

        result.Should().ContainSingle(x => x.Name == "Dashboard");
    }

    [Fact]
    public async Task GetPermissionsByMenuId_WhenExists_ReturnsPermissions()
    {
        await using var context = InMemoryDbContextFactory.Create();

        var menu = new Menu { Name = "Dashboard", Position = 1, Url = "/dash", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        context.Menus.Add(menu);
        await context.SaveChangesAsync();

        context.Permissions.Add(new Permission { Name = "Read", Slug = "read", MenuId = menu.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var repo = new PermissionRepository(context);
        var result = await repo.GetPermissionsByMenuId(menu.Id);

        result.Should().ContainSingle(x => x.Name == "Read");
    }
}
