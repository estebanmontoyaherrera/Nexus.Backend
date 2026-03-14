using FluentAssertions;
using Identity.Application.UseCases.Users.Queries.GetAllQuery;
using Identity.Domain.Entities;
using Identity.Infrastructure.Services;
using Identity.Tests.Fixtures;

namespace Identity.Tests.UnitTests.Application;

public class GetAllUserHandlerTests
{
    [Fact]
    public async Task Handle_WhenUsersExist_ReturnsPagedResult()
    {
        await using var context = InMemoryDbContextFactory.Create();
        context.Users.AddRange(
            new User { FirstName = "A", LastName = "L", Email = "a@test.com", Password = "x", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow },
            new User { FirstName = "B", LastName = "K", Email = "b@test.com", Password = "x", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var sut = new GetAllUserHandler(new UnitOfWork(context), new OrderingQuery());
        var result = await sut.Handle(new GetAllUserQuery { NumPage = 1, NumRecordsPage = 10 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.TotalRecords.Should().Be(2);
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenStateFilterApplied_ReturnsFilteredUsers()
    {
        await using var context = InMemoryDbContextFactory.Create();
        context.Users.AddRange(
            new User { FirstName = "A", LastName = "L", Email = "a@test.com", Password = "x", State = "1", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow },
            new User { FirstName = "B", LastName = "K", Email = "b@test.com", Password = "x", State = "0", AuditCreateUser = 1, AuditCreateDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var sut = new GetAllUserHandler(new UnitOfWork(context), new OrderingQuery());
        var result = await sut.Handle(new GetAllUserQuery { StateFilter = "1", NumPage = 1, NumRecordsPage = 10 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_WhenNoRecords_ReturnsEmptyCollection()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var sut = new GetAllUserHandler(new UnitOfWork(context), new OrderingQuery());

        var result = await sut.Handle(new GetAllUserQuery { NumPage = 1, NumRecordsPage = 10 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEmpty();
        result.TotalRecords.Should().Be(0);
    }
}
