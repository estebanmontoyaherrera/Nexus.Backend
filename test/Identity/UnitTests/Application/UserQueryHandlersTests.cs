using FluentAssertions;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Users.Queries.GetByIdQuery;
using Identity.Application.UseCases.Users.Queries.GetSelectQuery;
using Identity.Application.UseCases.Users.Queries.UserRolePermissionsQuery;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class UserQueryHandlersTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IUserRepository> _repo = new();

    public UserQueryHandlersTests()
    {
        _uow.SetupGet(x => x.User).Returns(_repo.Object);
    }

    [Fact]
    public async Task Handle_GetUserById_WhenExists_ReturnsSuccess()
    {
        _repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User
        {
            Id = 1,
            FirstName = "A",
            LastName = "B",
            Email = "a@b.com",
            Password = "x",
            State = "1"
        });

        var sut = new GetUserByIdHandler(_uow.Object);
        var result = await sut.Handle(new GetUserByIdQuery { UserId = 1 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.UserId.Should().Be(1);
    }

    [Fact]
    public async Task Handle_GetUserSelect_WhenUsersAvailable_ReturnsItems()
    {
        _repo.Setup(x => x.GetAllAsync()).ReturnsAsync([
            new User { Id = 1, FirstName = "A", LastName = "B", Email = "a@b.com", Password = "x", State = "1" }
        ]);

        var sut = new GetUserSelectHandler(_uow.Object);
        var result = await sut.Handle(new GetUserSelectQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_GetUserWithRoleAndPermissions_WhenMissing_ReturnsFailure()
    {
        _repo.Setup(x => x.GetUserWithRoleAndPermissionsAsync(77)).ReturnsAsync((UserWithRoleAndPermissionsDto)null!);

        var sut = new GetUserWithRoleAndPermissionsHandler(_uow.Object);
        var result = await sut.Handle(new GetUserWithRoleAndPermissionsQuery { UserId = 77 }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }
}
