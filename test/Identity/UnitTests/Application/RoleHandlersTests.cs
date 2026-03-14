using FluentAssertions;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Roles.Commands.CreateCommand;
using Identity.Application.UseCases.Roles.Commands.DeleteCommand;
using Identity.Application.UseCases.Roles.Commands.UpdateCommand;
using Identity.Application.UseCases.Roles.Queries.GetByIdQuery;
using Identity.Application.UseCases.Roles.Queries.GetSelectQuery;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class RoleHandlersTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IGenericRepository<Role>> _roleRepo = new();
    private readonly Mock<IPermissionRepository> _permissionRepo = new();
    private readonly Mock<IMenuRepository> _menuRepo = new();

    public RoleHandlersTests()
    {
        _uow.SetupGet(x => x.Role).Returns(_roleRepo.Object);
        _uow.SetupGet(x => x.Permission).Returns(_permissionRepo.Object);
        _uow.SetupGet(x => x.Menu).Returns(_menuRepo.Object);
        _uow.Setup(x => x.BeginTransaction()).Returns(Mock.Of<System.Data.IDbTransaction>());
    }

    [Fact]
    public async Task Handle_CreateRole_WithValidRequest_ReturnsSuccess()
    {
        var sut = new CreateRoleHandler(_uow.Object);
        var command = new CreateRoleCommand
        {
            Name = "Admin",
            State = "1",
            Menus = [new MenuRequestDto { MenuId = 1 }],
            Permissions = [new PermissionRequestDto { PermissionId = 1, PermissionName = "Read", PermissionDescription = "Read", Selected = true }]
        };

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_DeleteRole_WhenRoleExists_ReturnsSuccess()
    {
        _roleRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Role { Name = "Admin" });

        var sut = new DeleteRoleHandler(_uow.Object);
        var result = await sut.Handle(new DeleteRoleCommand { RoleId = 1 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _roleRepo.Verify(x => x.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Handle_GetRoleById_WhenNotFound_ReturnsFailure()
    {
        _roleRepo.Setup(x => x.GetByIdAsync(55)).ReturnsAsync((Role)null!);
        var sut = new GetRoleByIdHandler(_uow.Object);

        var result = await sut.Handle(new GetRoleByIdQuery { RoleId = 55 }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_GetRoleSelect_WhenRepositoryThrows_ReturnsFailure()
    {
        _roleRepo.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("role-read-fail"));
        var sut = new GetRoleSelectHandler(_uow.Object);

        var result = await sut.Handle(new GetRoleSelectQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("role-read-fail");
    }

    [Fact]
    public async Task Handle_UpdateRole_WithValidRequest_ReturnsSuccess()
    {
        _permissionRepo.Setup(x => x.GetPermissionRolesByRoleId(1)).ReturnsAsync([]);
        _menuRepo.Setup(x => x.GetMenuRolesByRoleId(1)).ReturnsAsync([]);

        var sut = new UpdateRoleHandler(_uow.Object);
        var command = new UpdateRoleCommand
        {
            RoleId = 1,
            Name = "Admin",
            State = "1",
            Menus = [new MenuUpdateRequestDto { MenuId = 1 }],
            Permissions = [new PermissionUpdateRequestDto { PermissionId = 1, PermissionName = "Read", PermissionDescription = "Read", Selected = true }]
        };

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
}
