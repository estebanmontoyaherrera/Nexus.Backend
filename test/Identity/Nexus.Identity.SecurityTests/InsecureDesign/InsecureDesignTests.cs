using FluentAssertions;
using FluentValidation;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Application.UseCases.Users.Queries.LoginQuery;

namespace Nexus.Identity.SecurityTests.InsecureDesign;

public class InsecureDesignTests
{
    [Fact]
    public void InsecureDesign_LoginFlow_ShouldNotHaveDedicatedFluentValidator_DetectedWeakness()
    {
        var validatorType = typeof(LoginQuery).Assembly.GetTypes()
            .FirstOrDefault(t =>
                !t.IsAbstract &&
                typeof(IValidator<LoginQuery>).IsAssignableFrom(t));

        validatorType.Should().BeNull();
    }

    [Fact]
    public void InsecureDesign_CreateUserFlow_ShouldNotHaveDedicatedFluentValidator_DetectedWeakness()
    {
        var validatorType = typeof(CreateUserCommand).Assembly.GetTypes()
            .FirstOrDefault(t =>
                !t.IsAbstract &&
                typeof(IValidator<CreateUserCommand>).IsAssignableFrom(t));

        validatorType.Should().BeNull();
    }
}
