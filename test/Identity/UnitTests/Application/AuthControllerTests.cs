using FluentAssertions;
using Identity.Api.Controllers;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
using Moq;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Tests.UnitTests.Application;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_WithValidRequest_ReturnsOkWithResponse()
    {
        var dispatcher = new Mock<IDispatcher>();
        dispatcher.Setup(x => x.Dispatch<LoginQuery, string>(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BaseResponse<string> { IsSuccess = true, AccessToken = "token" });

        var controller = new AuthController(dispatcher.Object);

        var action = await controller.Login(new LoginQuery { Email = "test@test.com", Password = "P@ssw0rd" });

        action.Should().BeOfType<OkObjectResult>();
    }
}
