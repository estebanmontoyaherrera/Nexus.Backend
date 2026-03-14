using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Tests.IntegrationTests.Persistence;

public class GenericRepositoryIntegrationTests
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
    public async Task CreateAsync_WithEntity_PersistsRecord()
    {
        await using var context = CreateContext();
        var repository = new GenericRepository<Role>(context);

        var role = new Role { Name = "Tester", Description = "desc", State = "1" };
        await repository.CreateAsync(role);
        await context.SaveChangesAsync();

        context.Roles.Should().ContainSingle(x => x.Name == "Tester");
    }

    [Fact]
    public async Task DeleteAsync_WithEntity_MarksSoftDeleteFields()
    {
        await using var context = CreateContext();
        var repository = new GenericRepository<Role>(context);

        var role = new Role { Name = "ToDelete", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(role.Id);
        await context.SaveChangesAsync();

        var deleted = await context.Roles.SingleAsync(x => x.Id == role.Id);
        deleted.AuditDeleteUser.Should().NotBeNull();
        deleted.AuditDeleteDate.Should().NotBeNull();
    }
}
