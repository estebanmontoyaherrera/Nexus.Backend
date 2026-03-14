using FluentAssertions;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Application.UseCases.Menus.Queries.GetByIdQuery;
using Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.UnitTests.Application;

public class MenuAndPermissionHandlersTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IMenuRepository> _menuRepo = new();
    private readonly Mock<IPermissionRepository> _permissionRepo = new();

    public MenuAndPermissionHandlersTests()
    {
        _uow.SetupGet(x => x.Menu).Returns(_menuRepo.Object);
        _uow.SetupGet(x => x.Permission).Returns(_permissionRepo.Object);
    }

    [Fact]
    public async Task Handle_GetMenuByUserId_WhenMenusExist_ReturnsSuccess()
    {
        _menuRepo.Setup(x => x.GetMenuByUserIdAsync(1)).ReturnsAsync([
            new Menu { Id = 1, Name = "Admin", Url = "/admin", Position = 1, State = "1" }
        ]);

        var sut = new GetMenuByUserIdHandler(_uow.Object);
        var result = await sut.Handle(new GetMenuByUserIdQuery { UserId = 1 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_GetPermissionsByRoleId_WhenRoleProvided_MapsSelection()
    {
        _menuRepo.Setup(x => x.GetMenuPermissionByRoleIdAsync(1)).ReturnsAsync([
            new Menu { Id = 1, Name = "Admin", Icon = "i", Url = "/admin", Position = 1, State = "1" }
        ]);

        _permissionRepo.Setup(x => x.GetPermissionsByMenuId(1)).ReturnsAsync([
            new Permission { Id = 7, Name = "Create", Description = "Create", Slug = "create", MenuId = 1 }
        ]);

        _permissionRepo.Setup(x => x.GetRolePermissionsByMenuId(1, 1)).ReturnsAsync([
            new Permission { Id = 7, Name = "Create", Description = "Create", Slug = "create", MenuId = 1 }
        ]);

        var sut = new GetPermissionsByRoleIdHandler(_uow.Object);
        var result = await sut.Handle(new GetPermissionsByRoleIdQuery { RoleId = 1 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().ContainSingle();
        result.Data!.Single().Permissions.Single().Selected.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_GetPermissionsByRoleId_WhenMenuReadFails_ReturnsFailure()
    {
        _menuRepo.Setup(x => x.GetMenuPermissionByRoleIdAsync(It.IsAny<int?>())).ThrowsAsync(new Exception("permissions-failed"));

        var sut = new GetPermissionsByRoleIdHandler(_uow.Object);
        var result = await sut.Handle(new GetPermissionsByRoleIdQuery { RoleId = 1 }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("permissions-failed");
    }
}
