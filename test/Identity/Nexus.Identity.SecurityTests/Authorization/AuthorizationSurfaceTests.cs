using FluentAssertions;
using Identity.Api.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace Nexus.Identity.SecurityTests.Authorization;

public class AuthorizationSurfaceTests
{
    [Fact]
    public void Authorization_BypassRisk_UserController_ShouldLackAuthorizeAttribute_DetectedWeakness()
    {
        var hasAuthorize = typeof(UserController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).Any();

        hasAuthorize.Should().BeFalse("UserController currently exposes identity operations without an [Authorize] attribute");
    }

    [Fact]
    public void Authorization_ProtectedControllers_ShouldRequireAuthorizeAttribute()
    {
        var protectedControllers = new[]
        {
            typeof(RoleController),
            typeof(PermissionController),
            typeof(UserRoleController),
            typeof(MenuController)
        };

        protectedControllers.Should().OnlyContain(c =>
            c.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).Any());
    }
}
