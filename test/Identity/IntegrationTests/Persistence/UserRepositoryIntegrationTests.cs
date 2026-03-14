using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Tests.IntegrationTests.Persistence;

public class UserRepositoryIntegrationTests
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
    public async Task UserByEmailAsync_WithExistingActiveUser_ReturnsUser()
    {
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            FirstName = "Repo",
            LastName = "Test",
            Email = "repo@test.com",
            Password = "hash",
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var user = await repository.UserByEmailAsync("repo@test.com");

        user.Should().NotBeNull();
        user.Email.Should().Be("repo@test.com");
    }

    [Fact]
    public async Task GetUserWithRoleAndPermissionsAsync_WithExistingData_ReturnsRoleAndPermissions()
    {
        await using var context = CreateContext();

        var user = new User { FirstName = "John", LastName = "Doe", Email = "john@doe.com", Password = "hash", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        var role = new Role { Name = "Admin", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        var menu = new Menu { Name = "Config", Position = 1, Url = "/config", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };

        context.Users.Add(user);
        context.Roles.Add(role);
        context.Menus.Add(menu);
        await context.SaveChangesAsync();

        var permission = new Permission { Name = "Read", Slug = "read", MenuId = menu.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        context.Permissions.Add(permission);

        context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permission.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        context.MenuRoles.Add(new MenuRole { RoleId = role.Id, MenuId = menu.Id, State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });

        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var dto = await repository.GetUserWithRoleAndPermissionsAsync(user.Id);

        dto.Should().NotBeNull();
        dto.Role.Should().NotBeNull();
        dto.Role!.Permissions.Should().ContainSingle(p => p.Name == "Read");
    }
}
