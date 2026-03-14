using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Identity.Tests.UnitTests.Infrastructure;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_WithValidUser_ReturnsJwtString()
    {
        var options = Options.Create(new JwtSettings
        {
            Secret = "test-secret-key-1234567890-test-secret-key",
            Issuer = "Identity",
            Audience = "Identity",
            ExpiryMinutes = 60
        });

        var sut = new JwtTokenGenerator(options);
        var token = sut.GenerateToken(new User
        {
            Id = 5,
            FirstName = "Test",
            LastName = "User",
            Email = "test@local",
            Password = "hash"
        });

        token.Should().NotBeNullOrWhiteSpace();
        token.Split('.').Length.Should().Be(3);
    }

    [Fact]
    public void GenerateRefreshToken_WhenCalled_ReturnsRandomBase64Token()
    {
        var options = Options.Create(new JwtSettings
        {
            Secret = "test-secret-key-1234567890-test-secret-key",
            Issuer = "Identity",
            Audience = "Identity",
            ExpiryMinutes = 60
        });

        var sut = new JwtTokenGenerator(options);
        var token1 = sut.GenerateRefreshToken();
        var token2 = sut.GenerateRefreshToken();

        token1.Should().NotBeNullOrWhiteSpace();
        token2.Should().NotBe(token1);
    }
}
