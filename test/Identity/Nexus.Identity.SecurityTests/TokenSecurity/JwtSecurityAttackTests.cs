using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Nexus.Identity.SecurityTests.Utilities;
using Xunit;

namespace Nexus.Identity.SecurityTests.TokenSecurity;

public class JwtSecurityAttackTests
{
    [Fact]
    public void JwtAttack_ExpiredToken_ShouldBeRejected()
    {
        var token = JwtAttackHelper.BuildToken(ApiSecurityFactory.ValidSecret, ApiSecurityFactory.ValidIssuer, ApiSecurityFactory.ValidAudience, DateTime.UtcNow.AddMinutes(-5));
        var handler = new JwtSecurityTokenHandler();

        var action = () => handler.ValidateToken(token, ApiSecurityFactory.BuildTokenValidationParameters(), out _);

        action.Should().Throw<SecurityTokenExpiredException>();
    }

    [Fact]
    public void JwtAttack_TamperedPayload_ShouldFailValidation()
    {
        var valid = JwtAttackHelper.BuildToken(ApiSecurityFactory.ValidSecret, ApiSecurityFactory.ValidIssuer, ApiSecurityFactory.ValidAudience, DateTime.UtcNow.AddMinutes(30));
        var tampered = JwtAttackHelper.TamperPayload(valid);
        var handler = new JwtSecurityTokenHandler();

        var action = () => handler.ValidateToken(tampered, ApiSecurityFactory.BuildTokenValidationParameters(), out _);

        action.Should().Throw<SecurityTokenInvalidSignatureException>();
    }

    [Fact]
    public void JwtAttack_WrongSigningKey_ShouldFailValidation()
    {
        var attackerSigned = JwtAttackHelper.BuildToken("attacker-secret-attacker-secret-attacker", ApiSecurityFactory.ValidIssuer, ApiSecurityFactory.ValidAudience, DateTime.UtcNow.AddMinutes(30));
        var handler = new JwtSecurityTokenHandler();

        var action = () => handler.ValidateToken(attackerSigned, ApiSecurityFactory.BuildTokenValidationParameters(), out _);

        action.Should().Throw<SecurityTokenInvalidSignatureException>();
    }
}
