using Identity.Api.Controllers;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.Tests.Fixtures;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<AuthController>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:IdentityConnection"] = "Data Source=localhost;Database=IdentityTests;Trusted_Connection=True;",
                ["JwtSettings:Secret"] = "test-secret-key-1234567890-test-secret-key",
                ["JwtSettings:ExpiryMinutes"] = "60",
                ["JwtSettings:Issuer"] = "Identity",
                ["JwtSettings:Audience"] = "Identity"
            };

            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase($"IdentityIntegrationDb-{Guid.NewGuid()}"));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();

            SeedDatabase(db);
        });
    }

    private static void SeedDatabase(ApplicationDbContext db)
    {
        if (db.Users.Any()) return;

        var now = DateTime.UtcNow;

        var role = new Role
        {
            Name = "Admin",
            Description = "System administrator",
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        };

        var menu = new Menu
        {
            Name = "Administration",
            Icon = "settings",
            Url = "/admin",
            Position = 1,
            FatherId = null,
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        };

        var user = new User
        {
            FirstName = "Integration",
            LastName = "Tester",
            Email = "integration@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!"),
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        };

        db.Roles.Add(role);
        db.Menus.Add(menu);
        db.Users.Add(user);
        db.SaveChanges();

        var permission = new Permission
        {
            Name = "RegisterUser",
            Description = "Register user permission",
            Slug = "register-user",
            MenuId = menu.Id,
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        };

        db.Permissions.Add(permission);
        db.SaveChanges();

        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        });

        db.MenuRoles.Add(new MenuRole
        {
            MenuId = menu.Id,
            RoleId = role.Id,
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        });

        db.RolePermissions.Add(new RolePermission
        {
            RoleId = role.Id,
            PermissionId = permission.Id,
            State = "1",
            AuditCreateUser = 1,
            AuditCreateDate = now
        });

        db.SaveChanges();
    }
}
