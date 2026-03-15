using FluentAssertions;
using Identity.Api.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace Nexus.Identity.SecurityTests.AccessControl;

public class PrivilegeEscalationTests
{
    [Fact]
    public void PrivilegeEscalation_UserAssignAdminRole_ShouldLackRoleGuard_DetectedWeakness()
    {
        var method = typeof(UserRoleController).GetMethod("UserRoleCreate");
        var hasRoleBasedAuthorize = method!
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
            .Cast<AuthorizeAttribute>()
            .Any(attr => !string.IsNullOrWhiteSpace(attr.Roles));

        hasRoleBasedAuthorize.Should().BeFalse("role assignment endpoint lacks explicit role-based guardrails");
    }

    [Fact]
    public void PrivilegeEscalation_UserController_DeleteOperation_ShouldNotDeclareAuthorize_DetectedWeakness()
    {
        var method = typeof(UserController).GetMethod("UserDelete");
        var methodAuthorize = method!.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();
        var controllerAuthorize = typeof(UserController).GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

        (methodAuthorize || controllerAuthorize).Should().BeFalse();
    }
}
