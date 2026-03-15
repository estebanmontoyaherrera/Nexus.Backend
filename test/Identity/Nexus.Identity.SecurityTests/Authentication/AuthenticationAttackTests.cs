using FluentAssertions;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
using Nexus.Identity.SecurityTests.Utilities;
using Xunit;

namespace Nexus.Identity.SecurityTests.Authentication;

public class AuthenticationAttackTests
{
    private readonly SecurityTestFixture _fixture = new();

    [Fact]
    public async Task AuthenticationAttack_InvalidPassword_ShouldReturnUnauthorizedStyleFailure()
    {
        var user = _fixture.BuildUser(email: "victim@nexus.local", passwordHash: BCrypt.Net.BCrypt.HashPassword("Correct#Pass1"));
        var unitOfWork = _fixture.BuildUnitOfWork(user);
        var handler = new LoginHandler(unitOfWork, new FakeJwtTokenGenerator());

        var result = await handler.Handle(new LoginQuery { Email = user.Email, Password = "Wrong#Pass1" }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.AccessToken.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticationAttack_MalformedLoginRequest_ShouldFailWithoutTokenIssuance()
    {
        var unitOfWork = _fixture.BuildUnitOfWork(existingUser: null);
        var handler = new LoginHandler(unitOfWork, new FakeJwtTokenGenerator());

        var result = await handler.Handle(new LoginQuery { Email = null!, Password = null! }, CancellationToken.None);

        result.IsSuccess.Should().NotBeTrue();
        result.AccessToken.Should().BeNull();
    }

    [Fact]
    public async Task AuthenticationAttack_BruteForceAttempt_ShouldNotTriggerLockoutDefense_DetectedWeakness()
    {
        var user = _fixture.BuildUser(email: "target@nexus.local", passwordHash: BCrypt.Net.BCrypt.HashPassword("Good#Password1"));
        var unitOfWork = _fixture.BuildUnitOfWork(user);
        var handler = new LoginHandler(unitOfWork, new FakeJwtTokenGenerator());

        for (var i = 0; i < 5; i++)
        {
            var result = await handler.Handle(new LoginQuery { Email = user.Email, Password = $"BadPassword-{i}" }, CancellationToken.None);
            result.IsSuccess.Should().BeFalse();
        }

        ((FakeUserRepository)unitOfWork.User).UserByEmailCalls.Should().Be(5);
    }
}
