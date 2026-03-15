using FluentAssertions;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
using Nexus.Identity.SecurityTests.Utilities;
using Xunit;

namespace Nexus.Identity.SecurityTests.InjectionAttacks;

public class InjectionAttackTests
{
    private readonly SecurityTestFixture _fixture = new();

    [Fact]
    public async Task SqlInjection_LoginEndpoint_ShouldBeRejected()
    {
        var user = _fixture.BuildUser(email: "normal@nexus.local");
        var unitOfWork = _fixture.BuildUnitOfWork(user);
        var handler = new LoginHandler(unitOfWork, new FakeJwtTokenGenerator());

        var result = await handler.Handle(new LoginQuery
        {
            Email = MaliciousPayloads.SqlInjectionClassic,
            Password = "any"
        }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task XssInjection_CreateUser_ShouldPersistUnsanitizedPayload_DetectedWeakness()
    {
        var unitOfWork = _fixture.BuildUnitOfWork(existingUser: null);
        var handler = new CreateUserHandler(unitOfWork);

        var result = await handler.Handle(new CreateUserCommand
        {
            FirstName = MaliciousPayloads.XssScriptTag,
            LastName = "Tester",
            Email = "xss@nexus.local",
            Password = "Secure#Password1",
            State = "1"
        }, CancellationToken.None);

        var created = ((FakeUserRepository)unitOfWork.User).LastCreatedUser;
        result.IsSuccess.Should().BeTrue();
        created.Should().NotBeNull();
        created!.FirstName.Should().Be(MaliciousPayloads.XssScriptTag);
    }
}
