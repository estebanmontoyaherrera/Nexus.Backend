using FluentAssertions;
using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;

namespace Identity.Tests.UnitTests.Infrastructure;

public class PermissionAuthorizationHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_WhenPermissionExists_Succeeds()
    {
        var permissionService = new Mock<IPermissionService>();
        permissionService.Setup(x => x.GetPermissionAsync(1)).ReturnsAsync(new HashSet<string> { "RegisterUser" });

        var services = new ServiceCollection();
        services.AddScoped(_ => permissionService.Object);
        var provider = services.BuildServiceProvider();

        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        var handler = new PermissionAuthorizationHandler(scopeFactory);

        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "1")
        ], "test"));

        var requirement = new PermissionRequirement("RegisterUser");
        var context = new AuthorizationHandlerContext([requirement], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }
}
