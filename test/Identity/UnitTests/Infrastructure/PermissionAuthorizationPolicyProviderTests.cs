using FluentAssertions;
using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Identity.Tests.UnitTests.Infrastructure;

public class PermissionAuthorizationPolicyProviderTests
{
    [Fact]
    public async Task GetPolicyAsync_WithUnknownPolicy_CreatesPermissionPolicy()
    {
        var options = Options.Create(new AuthorizationOptions());
        var sut = new PermissionAuthorizationPolicyProvider(options);

        var policy = await sut.GetPolicyAsync("RegisterUser");

        policy.Should().NotBeNull();
        policy!.Requirements.Should().ContainSingle(x => x is PermissionRequirement);
    }
}
