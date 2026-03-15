using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Nexus.Identity.SecurityTests.Utilities;

namespace Nexus.Identity.SecurityTests.ReplayAttacks;

public class ReplayAttackTests
{
    [Fact]
    public void ReplayAttack_AccessToken_ReusedTwice_ShouldRemainValid_DetectedWeakness()
    {
        var token = JwtAttackHelper.BuildToken(
            ApiSecurityFactory.ValidSecret,
            ApiSecurityFactory.ValidIssuer,
            ApiSecurityFactory.ValidAudience,
            DateTime.UtcNow.AddMinutes(30));

        var handler = new JwtSecurityTokenHandler();
        var firstPrincipal = handler.ValidateToken(token, ApiSecurityFactory.BuildTokenValidationParameters(), out _);
        var secondPrincipal = handler.ValidateToken(token, ApiSecurityFactory.BuildTokenValidationParameters(), out _);

        firstPrincipal.Identity!.IsAuthenticated.Should().BeTrue();
        secondPrincipal.Identity!.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public void ReplayAttack_RevokeRefreshTokenEndpoint_ShouldBeUnauthenticatedSurface_DetectedWeakness()
    {
        var method = typeof(Identity.Api.Controllers.AuthController).GetMethod("RevokeRefreshToken");
        var hasAuthorize = method!.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), true).Any();

        hasAuthorize.Should().BeFalse();
    }
}
