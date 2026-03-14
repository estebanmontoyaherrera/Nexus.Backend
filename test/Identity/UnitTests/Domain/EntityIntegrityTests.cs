using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Tests.Builders;

namespace Identity.Tests.UnitTests.Domain;

public class EntityIntegrityTests
{
    [Fact]
    public void UserBuilder_Build_WithDefaults_ShouldPopulateRequiredFields()
    {
        var user = new UserBuilder().Build();

        user.FirstName.Should().NotBeNullOrWhiteSpace();
        user.LastName.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().Contain("@");
        user.Password.Should().NotBeNullOrWhiteSpace();
        user.State.Should().Be("1");
    }

    [Fact]
    public void RoleBuilder_Build_WithOverrides_ShouldApplyState()
    {
        var role = new RoleBuilder().WithName("Admin").WithState("0").Build();

        role.Name.Should().Be("Admin");
        role.State.Should().Be("0");
    }

    [Fact]
    public void PermissionBuilder_Build_WithMenuId_ShouldKeepAssociation()
    {
        var permission = new PermissionBuilder().WithMenuId(25).Build();

        permission.MenuId.Should().Be(25);
        permission.Slug.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void BaseEntity_WhenAuditFieldsSet_ShouldPreserveValues()
    {
        var userRole = new UserRole
        {
            UserId = 10,
            RoleId = 20,
            User = new User { FirstName = "A", LastName = "B", Email = "a@b.c", Password = "x" },
            Role = new Role { Name = "Admin" },
            State = "1",
            AuditCreateUser = 99,
            AuditCreateDate = DateTime.UtcNow
        };

        userRole.UserId.Should().Be(10);
        userRole.RoleId.Should().Be(20);
        userRole.AuditCreateUser.Should().Be(99);
    }
}
