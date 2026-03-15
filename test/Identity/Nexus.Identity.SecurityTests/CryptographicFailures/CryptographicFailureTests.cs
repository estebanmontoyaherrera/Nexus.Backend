using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Xunit;

namespace Nexus.Identity.SecurityTests.CryptographicFailures;

public class CryptographicFailureTests
{
    [Fact]
    public void CryptographicFailure_JwtClaims_SubWithoutNameIdentifier_ShouldBreakIdentityBinding()
    {
        var generator = new JwtTokenGenerator(Options.Create(new JwtSettings
        {
            Secret = "=guucg=-z_0g%)l7uw-a5#h3-gf%(92e73z(x_rn*-#g11jtvj",
            Issuer = "Identity",
            Audience = "Identity",
            ExpiryMinutes = 60
        }));

        var token = generator.GenerateToken(new User
        {
            Id = 77,
            FirstName = "Test",
            LastName = "User",
            Email = "token@nexus.local",
            Password = "hashed",
            State = "1"
        });

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Any(c => c.Type == JwtRegisteredClaimNames.Sub).Should().BeTrue();
        jwt.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Should().BeFalse();
    }
}
