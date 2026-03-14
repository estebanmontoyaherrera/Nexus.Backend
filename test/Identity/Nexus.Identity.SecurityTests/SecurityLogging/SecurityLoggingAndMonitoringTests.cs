using FluentAssertions;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
using Nexus.Identity.SecurityTests.Utilities;

namespace Nexus.Identity.SecurityTests.SecurityLogging;

public class SecurityLoggingAndMonitoringTests
{
    [Fact]
    public async Task SecurityLogging_InternalExceptionMessage_ShouldBeReturnedToClient_DetectedWeakness()
    {
        const string sensitiveError = "SQL connection failed for server=prod-sql-01;user=sa;";
        var handler = new LoginHandler(new ThrowingUnitOfWork(sensitiveError), new FakeJwtTokenGenerator());

        var result = await handler.Handle(new LoginQuery
        {
            Email = "attack@nexus.local",
            Password = "password"
        }, CancellationToken.None);

        result.IsSuccess.Should().NotBeTrue();
        result.Message.Should().Be(sensitiveError);
    }
}
