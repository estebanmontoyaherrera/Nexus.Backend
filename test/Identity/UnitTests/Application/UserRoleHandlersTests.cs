using FluentAssertions;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.UserRoles.Commands.CreateCommand;
using Identity.Application.UseCases.UserRoles.Commands.DeleteCommand;
using Identity.Application.UseCases.UserRoles.Commands.UpdateCommand;
using Identity.Application.UseCases.UserRoles.Queries.GetByIdQuery;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class UserRoleHandlersTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IGenericRepository<UserRole>> _repo = new();

    public UserRoleHandlersTests()
    {
        _uow.SetupGet(x => x.UserRole).Returns(_repo.Object);
    }

    [Fact]
    public async Task Handle_CreateUserRole_WithValidRequest_ReturnsSuccess()
    {
        var sut = new CreateUserRoleHandler(_uow.Object);
        var result = await sut.Handle(new CreateUserRoleCommand { UserId = 1, RoleId = 2, State = "1" }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Verify(x => x.CreateAsync(It.IsAny<UserRole>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateUserRole_WithValidRequest_ReturnsSuccess()
    {
        var sut = new UpdateUserRoleHandler(_uow.Object);
        var result = await sut.Handle(new UpdateUserRoleCommand { UserRoleId = 3, UserId = 1, RoleId = 2, State = "1" }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Verify(x => x.UpdateAsync(It.Is<UserRole>(x => x.Id == 3)), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteUserRole_WhenMissing_ReturnsFailure()
    {
        _repo.Setup(x => x.GetByIdAsync(9)).ReturnsAsync((UserRole)null!);
        var sut = new DeleteUserRoleHandler(_uow.Object);

        var result = await sut.Handle(new DeleteUserRoleCommand { UserRoleId = 9 }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _repo.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_GetUserRoleById_WhenExists_ReturnsData()
    {
        _repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new UserRole
        {
            Id = 1,
            UserId = 1,
            RoleId = 2,
            User = new User { FirstName = "A", LastName = "B", Email = "a@b.com", Password = "x" },
            Role = new Role { Name = "Admin" }
        });

        var sut = new GetUserRoleByIdHandler(_uow.Object);
        var result = await sut.Handle(new GetUserRoleByIdQuery { UserRoleId = 1 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }
}
