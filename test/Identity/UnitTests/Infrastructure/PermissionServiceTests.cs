using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Tests.UnitTests.Infrastructure;

public class PermissionServiceTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:IdentityConnection"] = "Fake"
        }).Build();

        return new ApplicationDbContext(options, configuration);
    }

    [Fact]
    public async Task GetPermissionAsync_WhenUserHasRolePermissions_ReturnsUniquePermissionSet()
    {
        await using var context = CreateContext();

        var role = new Role { Name = "Admin", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        var menu = new Menu { Name = "A", Position = 1, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        var user = new User { FirstName = "U", LastName = "T", Email = "u@t.com", Password = "x", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };

        context.AddRange(role, menu, user);
        await context.SaveChangesAsync();

        var permission = new Permission { Name = "RegisterUser", Slug = "register-user", MenuId = menu.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        context.Permissions.Add(permission);
        await context.SaveChangesAsync();

        context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permission.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var sut = new PermissionService(context);
        var permissions = await sut.GetPermissionAsync(user.Id);

        permissions.Should().Contain("RegisterUser");
    }
}
